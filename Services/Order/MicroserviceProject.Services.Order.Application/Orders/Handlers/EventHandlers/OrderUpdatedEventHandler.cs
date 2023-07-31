﻿using MediatR;
using MicroserviceProject.Services.Order.Domain.Events;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.EventHandlers;

public class OrderUpdatedEventHandler: INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent} - OrderId: {OrderID}", notification.GetType().Name,notification.Order.Id);
        
        return Task.CompletedTask;
    }
}