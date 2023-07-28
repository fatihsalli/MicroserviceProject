using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, CustomResponse<bool>>
{
    private readonly Config _config;
    private readonly IOrderDbContext _context;
    private readonly KafkaProducer _kafkaProducer;

    public DeleteOrderCommandHandler(IOrderDbContext context, IOptions<Config> config,KafkaProducer kafkaProducer)
    {
        _context = context;
        _config = config.Value;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<CustomResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            _context.Orders.Remove(order);

            order.AddDomainEvent(new OrderDeletedEvent(order));

            await _context.SaveChangesAsync(cancellationToken);

            // Kafka ile gönderme işini event tarafında yapabiliriz.
            // Mesajı kafkaya gönderiyoruz.
            var orderResponseForElastic = new OrderResponseForElastic
            {
                OrderId = order.Id,
                Status = "Deleted"
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
                    Log.Information(ex, "DeleteTodoItemCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "DeleteTodoItemCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}