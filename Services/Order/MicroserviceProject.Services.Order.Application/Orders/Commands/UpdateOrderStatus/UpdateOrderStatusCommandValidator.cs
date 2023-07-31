using FluentValidation;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;
using MicroserviceProject.Shared.Helpers;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandValidator: AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(Helpers.BeValidGuid).WithMessage("{PropertyName} must be uuid");

        RuleFor(x => x.StatusId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} must be int value");
    }
}