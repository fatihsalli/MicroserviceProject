﻿using FluentValidation;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Length(32).WithMessage("{PropertyName} must be uuid");
        
        RuleFor(x => x.Address).SetValidator(new AddressCreateValidator());
        
        RuleForEach(x => x.OrderItems).SetValidator(new OrderItemRequestValidator());
    }
}

public class AddressCreateValidator : AbstractValidator<AddressRequest>
{
    public AddressCreateValidator()
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
        
        RuleFor(x => x.Province)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
    }
}
public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Length(32).WithMessage("{PropertyName} must be uuid");
        
        RuleFor(x => x.ProductName)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} 's maximum lenght is 50");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
            .LessThanOrEqualTo(int.MaxValue).WithMessage("{PropertyName} must be less than (int.MaxValue)");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("{PropertyName} must be less than (decimal.MaxValue)");

    }
}