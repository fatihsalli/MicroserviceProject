using FluentValidation;
using MicroserviceProject.Services.Order.Application.Common.Helpers;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(Helpers.BeValidGuid).WithMessage("{PropertyName} must be uuid");
    }
}