using MediatR;
using MicroserviceProject.Services.Order.Domain.Events;
using Microsoft.Extensions.Logging;

namespace MicroserviceProject.Services.Order.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        string domainEventName = notification.GetType().Name;
        
        Console.WriteLine($"CleanArchitecture Domain Event: {domainEventName}");

        return Task.CompletedTask;
    }
}