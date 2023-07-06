using MicroserviceProject.Shared.BaseRepository;
using MicroserviceProject.Shared.BaseService;
using MicroserviceProject.Shared.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderModel = MicroserviceProject.Shared.Models.Order;


namespace MicroserviceProject.Order.Service;

public class OrderService : GenericService<OrderModel>, IOrderService
{
    public OrderService(IGenericRepository<OrderModel> repository) : base(repository)
    {
        
    }
}