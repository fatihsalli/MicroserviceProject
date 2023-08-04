using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.OrderElastic.Services.Interfaces;

public interface IOrderEventService
{
    Task<OrderResponse> GetOrderWithHttpClientAsync(string orderId);
}