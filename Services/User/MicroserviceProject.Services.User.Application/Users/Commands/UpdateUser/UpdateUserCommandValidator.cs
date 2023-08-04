using FluentValidation;
using MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;
using MicroserviceProject.Shared.Utilities.Validation;

namespace MicroserviceProject.Services.User.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(ValidationUtility.BeValidGuid).WithMessage("{PropertyName} must be uuid");
        
        RuleFor(x => x.Username)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
        
        RuleFor(x => x.Email)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(100).WithMessage("{PropertyName} 's maximum lenght is 100");
        
        RuleFor(x => x.Password)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(20).WithMessage("{PropertyName} 's maximum lenght is 20");
        
        RuleFor(x => x.FullName)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(100).WithMessage("{PropertyName} 's maximum lenght is 100");

        RuleForEach(x => x.Addresses).SetValidator(new AddressRequestValidator());
    }
}