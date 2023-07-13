using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
