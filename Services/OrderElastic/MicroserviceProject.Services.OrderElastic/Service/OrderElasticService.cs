using Elasticsearch.Net;
using MicroserviceProject.Services.OrderElastic.Dtos;
using MicroserviceProject.Shared.Configs;
using Nest;
using Serilog;

namespace MicroserviceProject.Services.OrderElastic.Service;

public class OrderElasticService
{
    private readonly IElasticClient _client;

    public OrderElasticService(IElasticClient client)
    {
        _client = client;
    }

    public async Task SaveOrderToElasticsearch(OrderResponse order)
    {
        try
        {
            var indexResponse = await _client.IndexDocumentAsync(order);

            if (!indexResponse.IsValid)
                throw new Exception(indexResponse.DebugInformation);
            
            Log.Information("Order model successfully saved on elasticsearch. ID: {OrderId}",order.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SaveOrderToElasticsearch exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }

    public void DeleteOrderFromElasticsearch(string orderId)
    {
        try
        {
            // Elasticsearch'den silme işlemi
            var deleteResponse = _client.Delete<byte[]>(orderId, d => d
                    .Refresh(Refresh.True) // Elasticsearch'ten silindikten sonra hemen güncellemesi için
            );
            
            if (!deleteResponse.IsValid)
                throw new Exception(deleteResponse.DebugInformation);
            
            Log.Information("Order model successfully deleted from elasticsearch. ID: {OrderId}",orderId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DeleteOrderFromElasticsearch exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}