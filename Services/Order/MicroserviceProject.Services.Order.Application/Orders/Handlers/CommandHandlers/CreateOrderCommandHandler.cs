using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Enums;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CustomResponse<CreatedOrderResponse>>
{
    private readonly Config _config;
    private readonly IOrderDbContext _context;
    private readonly HttpClient _httpClient;

    public CreateOrderCommandHandler(IOrderDbContext context, HttpClient httpClient, IOptions<Config> config)
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
            var requestUrl = $"{_config.HttpClient.UserApi}/{request.UserId}";
            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new NotFoundException("order with userid", request.UserId);

            // Value object - Set edilmesini private ettiğimiz için constructordan set ediyoruz.
            var newAddress = new Address(
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.Zip,
                request.Address.Line);

            var newOrder = new Domain.Entities.Order();

            newOrder.Id = Guid.NewGuid().ToString();
            newOrder.UserId = request.UserId;
            newOrder.Address = newAddress;
            newOrder.StatusId = OrderStatus.Pending;
            newOrder.Status = OrderStatusHelper.OrderStatusString[(int)newOrder.StatusId];
            newOrder.Description = OrderStatusHelper.OrderStatusDescriptions[(int)newOrder.StatusId];
            newOrder.Done = false;

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Quantity, x.Price);
            });
            newOrder.TotalPrice = newOrder.OrderItems.Sum(x => x.Price * x.Quantity);

            newOrder.AddDomainEvent(new OrderCreatedEvent(newOrder));

            // Önce başarıyla kaydedip sonra eventleri publish ediyoruz.
            await _context.Orders.AddAsync(newOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _context.PublishDomainEvents();

            return CustomResponse<CreatedOrderResponse>.Success(201,
                new CreatedOrderResponse { OrderId = newOrder.Id });
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "CreateOrderCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "CreateOrderCommandHandler Exception");
                    throw new Exception("Something went wrong!");
            }
        }
    }
}