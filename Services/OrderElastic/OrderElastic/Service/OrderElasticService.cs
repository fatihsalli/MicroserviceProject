﻿using System.Text;
using OrderElastic.Dtos;
using Elasticsearch.Net;
using MicroserviceProject.Shared.Configs;
using Nest;

namespace OrderElastic.Service;

public class OrderElasticService
{
    private readonly Config _config;

    public OrderElasticService(Config config)
    {
        _config = config;
    }

    public void SaveOrderToElasticsearch(OrderResponse order)
    {
        // Elasticsearch bağlantısı ve indeksleme işlemi için yapılandırma ayarları
        var settings = new ConnectionSettings(new Uri(_config.Elasticsearch.Addresses["Address-1"]))
            .DefaultIndex(_config.Elasticsearch.IndexName["OrderSave"]);

        // Elasticsearch bağlantı nesnesi oluşturma
        var esClient = new ElasticClient(settings);

        // Sipariş nesnesini JSON formatına çevirme
        var jsonOrder = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(order));

        var indexResponse = esClient.Index<byte[]>(new IndexRequest<byte[]>(jsonOrder)
        {
            Refresh = Refresh.True // Elasticsearch'e kaydedildikten hemen sonra erişilebilir olmasını sağlar
        });
        
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
        var settings = new ConnectionSettings(new Uri(_config.Elasticsearch.Addresses["Address 1"]))
            .DefaultIndex(_config.Elasticsearch.IndexName["OrderSave"]);

        // Elasticsearch bağlantı nesnesi oluşturma
        var esClient = new ElasticClient(settings);

        // Elasticsearch'den silme işlemi
        var deleteResponse = esClient.Delete<byte[]>(orderId, d => d
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