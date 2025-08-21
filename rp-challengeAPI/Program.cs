using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using rp_challenge.Application.DTOs;
using rp_challenge.Application.Services;
using rp_challenge.Application.Validators;
using rp_challenge.Domain.Repositories;
using rp_challenge.Infraestructure.Data;
using rp_challenge.Infraestructure.Repositories;
using rp_challenge.Infraestructure.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car DealerShip API", Version = "v1" });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Database
builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    return new PostgreSqlConnectionFactory(connectionString);
});

builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

// Repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Validators
builder.Services.AddScoped<IValidator<CreateCarDTO>, CreateCarDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCarDTO>, UpdateCarDtoValidator>();
builder.Services.AddScoped<IValidator<CreateUserDTO>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginDtoValidator>();


// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await databaseInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();


// Auth endpoints
app.MapPost("/api/auth/register", async (CreateUserDTO request, IUserService userService) =>
{
    try
    {
        var user = await userService.CreateAsync(request);
        return Results.Created($"/api/users/{user.Id}", user);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("Register")
.WithTags("Authentication")
.WithOpenApi();

app.MapPost("/api/auth/login", async (LoginDTO request, IUserService userService) =>
{
    try
    {
        var response = await userService.LoginAsync(request);
        return Results.Ok(response);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
    catch (Exception)
    {
        return Results.Unauthorized();
    }
})
.WithName("Login")
.WithTags("Authentication")
.WithOpenApi();

// Car endpoints
app.MapGet("/api/cars", async (ICarService carService) =>
{
    var cars = await carService.GetAllAsync();
    return Results.Ok(cars);
})
.WithName("GetAllCars")
.WithTags("Cars")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/api/cars/{id:int}", async (int id, ICarService carService) =>
{
    var car = await carService.GetByIdAsync(id);
    return car == null ? Results.NotFound() : Results.Ok(car);
})
.WithName("GetCarById")
.WithTags("Cars")
.RequireAuthorization()
.WithOpenApi();

app.MapPost("/api/cars", async (CreateCarDTO request, ICarService carService) =>
{
    try
    {
        var car = await carService.CreateAsync(request);
        return Results.Created($"/api/cars/{car.Id}", car);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("CreateCar")
.WithTags("Cars")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/api/cars/{id:int}", async (int id, UpdateCarDTO request, ICarService carService) =>
{
    try
    {
        var car = await carService.UpdateAsync(id, request);
        return Results.Ok(car);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("UpdateCar")
.WithTags("Cars")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/api/cars/{id:int}", async (int id, ICarService carService) =>
{
    try
    {
        await carService.DeleteAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("DeleteCar")
.WithTags("Cars")
.RequireAuthorization()
.WithOpenApi();

// User endpoints
app.MapGet("/api/users/me", async (ClaimsPrincipal user, IUserService userService) =>
{
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
        return Results.Unauthorized();
    }

    var userData = await userService.GetByIdAsync(userId);
    return userData == null ? Results.NotFound() : Results.Ok(userData);
})
.WithName("GetCurrentUser")
.WithTags("Users")
.RequireAuthorization()
.WithOpenApi();

app.Run();