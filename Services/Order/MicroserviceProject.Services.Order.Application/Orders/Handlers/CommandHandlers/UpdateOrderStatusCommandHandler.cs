using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrderStatus;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Enums;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class UpdateOrderStatusCommandHandler: IRequestHandler<UpdateOrderStatusCommand, CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;
    private readonly KafkaProducer _kafkaProducer;
    private readonly Config _config;

    public UpdateOrderStatusCommandHandler(IOrderDbContext context,KafkaProducer kafkaProducer,IOptions<Config> config)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
        _config = config.Value;
    }
    
    public async Task<CustomResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            order.StatusId = (OrderStatus)request.StatusId;
            order.Status = OrderStatusHelper.OrderStatusString[(int)order.StatusId];
            order.Description = OrderStatusHelper.OrderStatusDescriptions[(int)order.StatusId];

            await _context.SaveChangesAsync(cancellationToken);
            
            // Kafka ile gönderme işini event tarafında yapabiliriz.
            // Mesajı kafkaya gönderiyoruz.
            var orderResponseForElastic = new OrderResponseForElastic
            {
                OrderId = order.Id,
                Status = "Updated"
            };

            var jsonKafkaMessage = JsonSerializer.Serialize(orderResponseForElastic);
            await _kafkaProducer.SendToKafkaWithMessageAsync(jsonKafkaMessage, _config.Kafka.TopicName["OrderID"]);

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "UpdateOrderCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "UpdateOrderCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}