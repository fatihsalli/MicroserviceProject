using FluentValidation;
using MicroserviceProject.Shared.Helpers;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;

public class GetOrdersByUserIdQueryValidator : AbstractValidator<GetOrdersByUserIdQuery>
{
    public GetOrdersByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(Helpers.BeValidGuid).WithMessage("{PropertyName} must be uuid");
    }
}

