using MediatR;
using MicroserviceProject.Services.Order.Domain.Events;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.EventHandlers;

public class OrderCompletedEventHandler : INotificationHandler<OrderCompletedEvent>
{
    public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}