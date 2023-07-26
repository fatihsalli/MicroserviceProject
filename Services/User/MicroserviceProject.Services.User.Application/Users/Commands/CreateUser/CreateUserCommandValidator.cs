using FluentValidation;
using MicroserviceProject.Services.User.Application.Dtos.Requests;

namespace MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
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

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(x => x.Province)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
        
        RuleFor(x => x.District)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
        
        RuleFor(x => x.Street)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
        
        RuleFor(x => x.Zip)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(10).WithMessage("{PropertyName} 's maximum lenght is 10");
        
        RuleFor(x => x.Line)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(100).WithMessage("{PropertyName} 's maximum lenght is 100");
    }
}