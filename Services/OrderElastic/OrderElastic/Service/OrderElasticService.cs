using System.Text;
using OrderElastic.Dtos;
using Elasticsearch.Net;
using MicroserviceProject.Shared.Configs;
using Nest;

namespace OrderElastic.Service;

public class OrderElasticService
{
    private readonly Config _config;
    private readonly IElasticClient _client;

    public OrderElasticService(Config config,IElasticClient client)
    {
        _config = config;
        _client = client;
    }

    public void SaveOrderToElasticsearch(OrderResponse order)
    {
        var indexResponse = _client.IndexDocument(order);

        // Hata kontrolü
        if (!indexResponse.IsValid)
        {
            Console.WriteLine($"Elasticsearch'e kayıt işlemi başarısız oldu. Hata: {indexResponse.DebugInformation}");
            // Burada hata durumuna göre işlemler yapabilirsiniz.
        }
        else
        {
            Console.WriteLine($"Sipariş Elasticsearch'e başarıyla kaydedildi. ID: {order.Id}");
            // Başarılı kayıt durumunda yapılacak işlemler
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