using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Services.Order.Domain.Events;
using Microsoft.Extensions.Logging;

namespace MicroserviceProject.Services.Order.Application.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<DomainEventNotification<OrderCreatedEvent>>
{
    public Task Handle(DomainEventNotification<OrderCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        
        string domainEventName = domainEvent.GetType().Name;
        
        Console.WriteLine($"CleanArchitecture Domain Event: {domainEventName}");

        return Task.CompletedTask;
    }
}