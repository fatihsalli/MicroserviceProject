using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.Events;

public class OrderCompletedEvent : BaseEvent
{
    public Entities.Order Order { get; }
    public OrderCompletedEvent(Entities.Order order)
    {
        Order = order;
    }
    
}