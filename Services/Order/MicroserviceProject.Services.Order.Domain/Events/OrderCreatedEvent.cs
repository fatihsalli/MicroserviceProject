using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.Events;

public class OrderCreatedEvent : BaseEvent
{
    public Entities.Order Order { get; }

    public OrderCreatedEvent(Entities.Order order)
    {
        Order = order;
    }
}