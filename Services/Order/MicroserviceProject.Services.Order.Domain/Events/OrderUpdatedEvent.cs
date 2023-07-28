using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.Events;

public class OrderUpdatedEvent : BaseEvent
{
    public OrderUpdatedEvent(Entities.Order order)
    {
        Order = order;
    }

    public Entities.Order Order { get; }
}