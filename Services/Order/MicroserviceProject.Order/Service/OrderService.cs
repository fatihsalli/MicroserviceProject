using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderModel=MicroserviceProject.Shared.Models.Order;


namespace MicroserviceProject.Order.Service;

public class OrderService:IOrderService
{
    private readonly Config _config;
    private readonly IGenericRepository<Shared.Models.Order> _repository;
    public OrderService(IOptions<Config> config)
    {
        _config = config.Value;
        var mongoClient = new MongoClient(_config.Database.Connection);
        var database = mongoClient.GetDatabase(_config.Database.DatabaseName);
        _repository = new GenericRepository<Shared.Models.Order>(database,_config.Database.OrderCollectionName);
    }

    public List<OrderModel> GetAll()
    {
        var orders = _repository.GetAll();
        return orders.ToList();
    }

    public OrderModel Create(OrderModel order)
    {
        order.Id = Guid.NewGuid().ToString();
        order.CreatedAt=DateTime.Now;
        order.UpdadetAt = order.CreatedAt;
        _repository.Insert(order);
        return order;
    }
    


}