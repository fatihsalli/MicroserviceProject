using System.Net.Http.Json;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Responses;
using OrderElastic.Dtos;
using Serilog;

namespace OrderElastic.Service;

public class OrderEventService
{
    private readonly Config _config;
    public OrderEventService(Config config)
    {
        _config = config;
    }
    
    public async Task<OrderResponse> GetOrderWithHttpClientAsync(string orderId)
    {
        var httpClient = new HttpClient();
        string requestUrl = $"{_config.HttpClient.OrderApi}/{orderId}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            var responseFail = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            Log.Error("Order with id ({OrderId}) cannot found!",orderId);
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<CustomResponse<OrderResponse>>();
        return responseSuccess.Data;
    }
}