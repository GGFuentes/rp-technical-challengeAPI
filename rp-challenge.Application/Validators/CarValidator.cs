using FluentValidation;
using rp_challenge.Application.DTOs;

namespace rp_challenge.Application.Validators
{
    public class CreateCarDtoValidator : AbstractValidator<CreateCarDTO>
    {
        public CreateCarDtoValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .MaximumLength(100)
                .WithMessage("Brand cannot exceed 200 characters");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required")
                .MaximumLength(100)
                .WithMessage("Model cannot exceed 100 characters");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required")
                .LessThan(10000)
                .WithMessage("Invalid Price format");
        }
    }

    public class UpdateCarDtoValidator : AbstractValidator<UpdateCarDTO>
    {
        public UpdateCarDtoValidator()
        {
            RuleFor(x => x.Brand)
                 .NotEmpty()
                 .WithMessage("Brand is required")
                 .MaximumLength(100)
                 .WithMessage("Brand cannot exceed 200 characters");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required")
                .MaximumLength(100)
                .WithMessage("Model cannot exceed 100 characters");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required")
                .LessThan(10000)
                .WithMessage("Invalid Price format");
        }
    }
}
