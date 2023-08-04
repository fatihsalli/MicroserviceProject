using System.Text.Json;
using MicroserviceProject.Services.OrderElastic.Services.Interfaces;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.OrderElastic.Roots;

public class OrderEventRoot
{
    private readonly Config _config;
    private readonly KafkaProducer _kafkaProducer;
    private readonly KafkaConsumer _kafkaConsumer;
    private readonly IOrderElasticService _orderElasticService;
    private readonly IOrderEventService _orderEventService;

    public OrderEventRoot
        (IOptions<Config> config, IOrderElasticService orderElasticService, IOrderEventService orderEventService)
    {
        _config = config.Value;
        _orderElasticService = orderElasticService;
        _orderEventService = orderEventService;
        _kafkaProducer = new KafkaProducer(_config.Kafka.Address);
        _kafkaConsumer = new KafkaConsumer(_config.Kafka.Address);
    }

    public async Task StartGetOrderAndPushOrderAsync()
    {
        // Dinlemek istediğimiz topic adını belirtiyoruz.
        var topics = new List<string> { _config.Kafka.TopicName["OrderID"] };
        _kafkaConsumer.SubscribeToTopics(topics);

        Log.Information("Order with topic [{TopicName}] is listening", _config.Kafka.TopicName["OrderID"]);

        while (true)
        {
            var messages = _kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 10,
                bulkConsumeMaxTimeoutInSeconds: 5, maxReadCount: 2);

            foreach (var message in messages)
            {
                var orderResponseForElastic = JsonSerializer.Deserialize<OrderResponseForElastic>(message.Value);
                Log.Information("Received message: {MessageValue}", message.Value);

                switch (orderResponseForElastic.Status)
                {
                    // Gelen mesaj status değeri "Created" veya "Updated" ise "HttpClient" ile order modelimi alıyorum. Bu order modelimi tekrar kafkaya gönderiyorum. Bunun sebebi de bu gönderdiğim ordermodeli birden fazla servis dinleyebilir ve işlem yapabilir.
                    case "Created":
                    case "Updated":
                        var orderResponse =
                            await _orderEventService.GetOrderWithHttpClientAsync(orderResponseForElastic.OrderId);
                        var jsonKafkaMessage = JsonSerializer.Serialize(orderResponse);
                        await _kafkaProducer.SendToKafkaWithMessageAsync(jsonKafkaMessage,
                            _config.Kafka.TopicName["OrderModel"]);
                        Log.Information(
                            "Pushed order model: {OrderResponseId} | Topic: {TopicName}", orderResponse.Id,
                            _config.Kafka.TopicName["OrderModel"]);
                        break;

                    // Gelen mesaj status değeri "Deleted" ise elasticsearch'den direkt olarak siliyorum.
                    case "Deleted":
                        _orderElasticService.DeleteOrderFromElasticsearch(orderResponseForElastic.OrderId);
                        break;
                    default:
                        Log.Error("Unknown order response status. | Status: {OrderStatus}",
                            orderResponseForElastic.Status);
                        break;
                }
            }

            // Aynı mesajların tekrar okunmaması için message offsetlerini commitleyip temizliyoruz.
            _kafkaConsumer.CommitOffsets();
        }
    }
}