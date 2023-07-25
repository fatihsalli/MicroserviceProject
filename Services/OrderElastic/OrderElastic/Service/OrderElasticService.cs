using System.Text;
using OrderElastic.Dtos;
using Elasticsearch.Net;
using MicroserviceProject.Shared.Configs;
using Nest;

namespace OrderElastic.Service;

public class OrderElasticService
{
    private readonly ElasticsearchConfig _config;
    
    public OrderElasticService()
    {
        _config = new ElasticsearchConfig
        {
            Addresses = new Dictionary<string, string>
            {
                {"Address-1", "http://localhost:9200"},
                // Eğer başka adresler varsa buraya ekleyebilirsiniz
            },
            IndexName = new Dictionary<string, string>
            {
                {"OrderSave", "order_duplicate_v01"},
                // Eğer başka index isimleri varsa buraya ekleyebilirsiniz
            }
        };
    }
    
    public void SaveOrderToElasticsearch(OrderResponse order)
        {
            // Elasticsearch bağlantısı ve indeksleme işlemi için yapılandırma ayarları
            var settings = new ConnectionSettings(new Uri(_config.Addresses["Address-1"]))
                .DefaultIndex(_config.IndexName["OrderSave"]);

            // Elasticsearch bağlantı nesnesi oluşturma
            var esClient = new ElasticClient(settings);

            // Sipariş nesnesini JSON formatına çevirme
            var jsonOrder = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(order));

            // Elasticsearch'e gönderme ve indeksleme işlemi
            var indexResponse = esClient.IndexDocument(jsonOrder);

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
            // Elasticsearch bağlantısı ve silme işlemi için yapılandırma ayarları
            var settings = new ConnectionSettings(new Uri(_config.Addresses["Address 1"]))
                .DefaultIndex(_config.IndexName["OrderSave"]);

            // Elasticsearch bağlantı nesnesi oluşturma
            var esClient = new ElasticClient(settings);

            // Elasticsearch'den silme işlemi
            var deleteResponse = esClient.Delete<byte[]>(orderId, d => d
                .Refresh(Refresh.True) // Elasticsearch'ten silindikten sonra hemen güncellemesi için
            );

            // Hata kontrolü
            if (!deleteResponse.IsValid)
            {
                Console.WriteLine($"Elasticsearch'den silme işlemi başarısız oldu. Hata: {deleteResponse.DebugInformation}");
                // Burada hata durumuna göre işlemler yapabilirsiniz.
            }
            else
            {
                Console.WriteLine($"Sipariş Elasticsearch'den başarıyla silindi. ID: {orderId}");
                // Başarılı silme durumunda yapılacak işlemler
            }
        }
}