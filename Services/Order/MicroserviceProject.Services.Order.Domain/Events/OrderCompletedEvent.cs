using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.Events;

public class OrderCompletedEvent:DomainEvent
{
    public OrderCompletedEvent(Entities.Order item)
    {
        Item = item;
    }

    public Entities.Order Item { get; }
}