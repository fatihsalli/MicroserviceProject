using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.OrderElastic.Services.Interfaces;

public interface IOrderElasticService
{
    Task SaveOrderToElasticsearch(OrderResponse order);
    void DeleteOrderFromElasticsearch(string orderId);
}