using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories;

public class OrderRepository : BaseRepository<Domain.Entities.Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
        
    }
}