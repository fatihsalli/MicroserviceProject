using MicroserviceProject.Services.Order.Repository.Interfaces.Repositories;

namespace MicroserviceProject.Services.Order.Repository;

public class OrderRepository : BaseRepository<Domain.Entities.Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
        
    }
}