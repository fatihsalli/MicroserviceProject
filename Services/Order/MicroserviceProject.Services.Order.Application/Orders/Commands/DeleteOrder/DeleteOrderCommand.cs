using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(string Id) : IRequest<CustomResponse<bool>>;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand,CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;
    private readonly Config _config;

    public DeleteOrderCommandHandler(IOrderDbContext context,IOptions<Config> config)
    {
        _context = context;
        _config = config.Value;
    }

    public async Task<CustomResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x=>x.Id==request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);

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
            var kafkaProducer = new KafkaProducer(_config.Kafka.Address);
            await kafkaProducer.SendToKafkaWithMessageAsync(jsonKafkaMessage,_config.Kafka.TopicName["OrderID"]);

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Information(ex, "DeleteTodoItemCommandHandler exception. Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            
            Log.Error(ex, "DeleteTodoItemCommandHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}