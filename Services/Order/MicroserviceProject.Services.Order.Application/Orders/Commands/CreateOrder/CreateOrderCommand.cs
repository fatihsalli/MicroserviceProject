
using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using Microsoft.Extensions.Options;
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
    private readonly Config _config;

    public CreateOrderCommandHandler(IOrderDbContext context,HttpClient httpClient,IOptions<Config> config)
    {
        _context = context;
        _httpClient = httpClient;
        _config = config.Value;
    }

    public async Task<CustomResponse<CreatedOrderResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // User Check
            // string requestUrl = $"{_config.HttpClient.UserApi}/{request.UserId}";
            // HttpResponseMessage response = await _httpClient.GetAsync(requestUrl,cancellationToken);
            //
            // if (!response.IsSuccessStatusCode)
            //     throw new NotFoundException("order with userid",request.UserId);
            
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
            
            // Mesajı kafkaya gönderiyoruz.
            var orderResponseForElastic = new OrderResponseForElastic
            {
                OrderId = newOrder.Id,
                Status = "Created"
            };

            var jsonKafkaMessage = JsonSerializer.Serialize(orderResponseForElastic);
            var kafkaProducer = new KafkaProducer(_config.Kafka.Address);
            await kafkaProducer.SendToKafkaWithMessage(jsonKafkaMessage,_config.Kafka.TopicName["OrderID"]);
            
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