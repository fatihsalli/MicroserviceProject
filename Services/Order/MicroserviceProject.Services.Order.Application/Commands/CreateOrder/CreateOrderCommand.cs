﻿using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Services.Order.Infrastructure;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Commands.CreateOrder;

public class CreateOrderCommand:IRequest<CustomResponse<CreatedOrderDto>>
{
    public string UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public AddressDto Address { get; set; }
}


public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CustomResponse<CreatedOrderDto>>
{
    private readonly OrderDbContext _context;
    public CreateOrderCommandHandler(OrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<CustomResponse<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(
            request.Address.Province,
            request.Address.District,
            request.Address.Street,
            request.Address.Zip,
            request.Address.Line);

        var newOrder = new Domain.Entities.Order(newAddress, request.UserId);

        request.OrderItems.ForEach(x => { newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Quantity, x.Price); });

        newOrder.DomainEvents.Add(new OrderCreatedEvent(newOrder));
        
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        return CustomResponse<CreatedOrderDto>.Success(201, new CreatedOrderDto { OrderId = newOrder.Id });
    }
}