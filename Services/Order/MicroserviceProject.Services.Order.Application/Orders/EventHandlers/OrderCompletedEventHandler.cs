using MediatR;
using MicroserviceProject.Services.Order.Domain.Events;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.EventHandlers;

public class OrderCompletedEventHandler : INotificationHandler<OrderCompletedEvent>
{
    public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
