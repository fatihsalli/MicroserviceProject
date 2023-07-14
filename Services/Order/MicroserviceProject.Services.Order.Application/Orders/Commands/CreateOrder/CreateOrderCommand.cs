using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Responses;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CustomResponse<CreatedOrderResponse>>
{
    public string UserId { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
    public AddressRequest Address { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CustomResponse<CreatedOrderResponse>>
{
    private readonly IOrderDbContext _context;

    public CreateOrderCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task<CustomResponse<CreatedOrderResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var newAddress = new Address(
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.Zip,
                request.Address.Line);

            var newOrder = new Domain.Entities.Order(newAddress, request.UserId, false)
            {
                Id = Guid.NewGuid().ToString()
            };

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Quantity, x.Price);
            });
            newOrder.TotalPrice = request.OrderItems.Sum(x => x.Price * x.Quantity);

            newOrder.AddDomainEvent(new OrderCreatedEvent(newOrder));

            await _context.Orders.AddAsync(newOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponse<CreatedOrderResponse>.Success(201, new CreatedOrderResponse { OrderId = newOrder.Id });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CreateOrderCommandHandler Exception");
            throw new Exception($"Something went wrong!");
        }
    }
}