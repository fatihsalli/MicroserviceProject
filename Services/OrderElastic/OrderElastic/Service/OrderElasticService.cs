using OrderElastic.Dtos;
using Elasticsearch.Net;
using MicroserviceProject.Shared.Configs;
using Nest;
using Serilog;

namespace OrderElastic.Service;

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
                throw new Exception(indexResponse.ServerError.Error.Reason);
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
        // Elasticsearch'den silme işlemi
        var deleteResponse = _client.Delete<byte[]>(orderId, d => d
                .Refresh(Refresh.True) // Elasticsearch'ten silindikten sonra hemen güncellemesi için
        );

        // Hata kontrolü
        if (!deleteResponse.IsValid)
        {
            Console.WriteLine(
                $"Elasticsearch'den silme işlemi başarısız oldu. Hata: {deleteResponse.DebugInformation}");
            // Burada hata durumuna göre işlemler yapabilirsiniz.
        }
        else
        {
            Console.WriteLine($"Sipariş Elasticsearch'den başarıyla silindi. ID: {orderId}");
            // Başarılı silme durumunda yapılacak işlemler
        }
    }
}