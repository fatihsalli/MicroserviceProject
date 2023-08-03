using System.Text.Json;
using MicroserviceProject.Services.OrderElastic.Dtos;
using MicroserviceProject.Services.OrderElastic.Services.Interfaces;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.OrderElastic.Roots;

/// <summary>
/// OrderElasticRoot sınıfım Kafka'ya "OrderEventRoot" tarafından gönderilen OrderResponse modelimi içeren mesajımı dinlemektir. Bu mesajı aldıktan sonra da elasticsearch'e kayıt (Create veya update) işlemini yapmaktadır. 
/// </summary>
public class OrderElasticRoot
{
    private readonly Config _config;
    private readonly KafkaConsumer _kafkaConsumer;
    private readonly IOrderElasticService _orderElasticService;

    public OrderElasticRoot(IOptions<Config> config, IOrderElasticService orderElasticService)
    {
        _config = config.Value;
        _kafkaConsumer = new KafkaConsumer(_config.Kafka.Address);
        _orderElasticService = orderElasticService;
    }

    public async Task StartConsumeAndSaveOrderAsync()
    {
        // Dinlemek istediğimiz topic adını belirtiyoruz.
        var topics = new List<string> { _config.Kafka.TopicName["OrderModel"] };
        _kafkaConsumer.SubscribeToTopics(topics);

        Log.Information("Order with topic [{TopicName}] is listening", _config.Kafka.TopicName["OrderModel"]);

        while (true)
        {
            var messages = _kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 10,
                bulkConsumeMaxTimeoutInSeconds: 5, maxReadCount: 2);

            foreach (var message in messages)
            {
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(message.Value);
                Log.Information("Received message: {MessageValue}", message.Value);
                await _orderElasticService.SaveOrderToElasticsearch(orderResponse);
            }

            // Aynı mesajların tekrar okunmaması için message offsetlerini commitleyip temizliyoruz.
            _kafkaConsumer.CommitOffsets();
        }
    }
}