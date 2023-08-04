using FluentValidation;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Shared.Utilities.Validation;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(ValidationUtility.BeValidGuid).WithMessage("{PropertyName} must be uuid");

        RuleFor(x => x.Address).SetValidator(new AddressRequestValidator());
    }
}