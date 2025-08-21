using FluentValidation;
using rp_challenge.Application.DTOs;
using rp_challenge.Domain.Entities;
using rp_challenge.Domain.Exception;
using rp_challenge.Infraestructure.Repositories;

namespace rp_challenge.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserDTO> _createValidator;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        public UserService(
            IUserRepository userRepository,
            IValidator<CreateUserDTO> createValidator,
            IValidator<LoginDTO> loginValidator,
            IJwtService jwtService,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _createValidator = createValidator;
            _loginValidator = loginValidator;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO createUserDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Check if email already exists
            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
            {
                throw new UserAlreadyExistsException(createUserDto.Email);
            }

            // Check if username already exists
            if (await _userRepository.ExistsByUsernameAsync(createUserDto.Name))
            {
                throw new ArgumentException($"Name {createUserDto.Name} already exists");
            }

            var password = _passwordService.HashPassword(createUserDto.Password);
            var user = User.Create(createUserDto.Email, createUserDto.Name, password);

            var userId = await _userRepository.CreateAsync(user);
            var createdUser = await _userRepository.GetByIdAsync(userId);
            return MapToDto(createdUser!);
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            var userDto = MapToDto(user);
            var token = _jwtService.GenerateToken(userDto);

            return new LoginResponseDTO
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        private static UserDTO MapToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Created = user.Created,
                Updated = user.Updated
            };
        }
    }
}
