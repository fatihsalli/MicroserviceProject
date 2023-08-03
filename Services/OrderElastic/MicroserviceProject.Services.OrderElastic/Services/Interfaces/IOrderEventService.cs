using MicroserviceProject.Services.OrderElastic.Dtos;

namespace MicroserviceProject.Services.OrderElastic.Services.Interfaces;

public interface IOrderEventService
{
    Task<OrderResponse> GetOrderWithHttpClientAsync(string orderId);
}