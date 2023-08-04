using System.Net.Http.Json;
using MicroserviceProject.Services.OrderElastic.Services.Interfaces;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.OrderElastic.Services;

public class OrderEventService : IOrderEventService
{
    private readonly Config _config;

    public OrderEventService(IOptions<Config> config)
    {
        _config = config.Value;
    }

    public async Task<OrderResponse> GetOrderWithHttpClientAsync(string orderId)
    {
        var httpClient = new HttpClient();
        string requestUrl = $"{_config.HttpClient.OrderApi}/{orderId}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            var responseFail = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            Log.Error("Order with id ({OrderId}) cannot found!", orderId);
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<CustomResponse<OrderResponse>>();
        return responseSuccess.Data;
    }
}