using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.Events;

public class OrderDeletedEvent : BaseEvent
{
    public OrderDeletedEvent(Entities.Order order)
    {
        Order = order;
    }

    public Entities.Order Order { get; }
}
