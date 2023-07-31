using FluentValidation;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;

public class GetOrdersByStatusQueryValidator:AbstractValidator<GetOrdersByStatusQuery>
{
    public GetOrdersByStatusQueryValidator()
    {
        RuleFor(x => x.StatusId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} must be int value");
    }
}