using System.Text.Json;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using OrderElastic.Dtos;
using OrderElastic.Service;
using Serilog;

namespace OrderElastic.Roots;

/// <summary>
/// OrderElasticRoot sınıfım Kafka'ya "OrderEventRoot" tarafından gönderilen OrderResponse modelimi içeren mesajımı dinlemektir. Bu mesajı aldıktan sonra da elasticsearch'e kayıt (Create veya update) işlemini yapmaktadır. 
/// </summary>
public class OrderElasticRoot
{
    private readonly Config _config;
    private readonly KafkaConsumer _kafkaConsumer;
    private readonly OrderElasticService _orderElasticService;

    public OrderElasticRoot()
    {
        var setup = new Setup.Setup();
        _config = setup.CreateConfig();
        _kafkaConsumer = setup.CreateKafkaConsumer(_config);
        _orderElasticService = setup.CreateOrderElasticService(_config);
    }

    public async Task StartConsumeAndSaveOrderAsync()
    {
        // Dinlemek istediğimiz topic adını belirtiyoruz.
        var topics = new List<string> { _config.Kafka.TopicName["OrderModel"] };
        _kafkaConsumer.SubscribeToTopics(topics);

        while (true)
        {
            var messages = _kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 10,
                bulkConsumeMaxTimeoutInSeconds: 5, maxReadCount: 2);

            foreach (var message in messages)
            {
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(message.Value);
                Log.Information($"Received message: {message.Value}");
                _orderElasticService.SaveOrderToElasticsearch(orderResponse);
                Log.Information($"Order model saved on elasticsearch: {orderResponse.Id}");
            }
            
            // Aynı mesajların tekrar okunmaması için message offsetlerini commitleyip temizliyoruz.
            _kafkaConsumer.CommitOffsets();
        }
    }
}