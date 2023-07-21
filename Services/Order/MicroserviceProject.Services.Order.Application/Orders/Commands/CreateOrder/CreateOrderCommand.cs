using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Exceptions;
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
    private readonly HttpClient _httpClient;

    public CreateOrderCommandHandler(IOrderDbContext context,HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<CustomResponse<CreatedOrderResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // User Check
            string userMicroserviceBaseUrl = "http://localhost:5012/api/users/";
            string requestUrl = $"{userMicroserviceBaseUrl}{request.UserId}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl,cancellationToken);
            
            if (!response.IsSuccessStatusCode)
                throw new NotFoundException("order with userid",request.UserId);
            
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
            if (ex is NotFoundException)
            {
                Log.Information(ex, "CreateOrderCommandHandler exception. Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            
            Log.Error(ex, "CreateOrderCommandHandler Exception");
            throw new Exception($"Something went wrong!");
        }
    }
}