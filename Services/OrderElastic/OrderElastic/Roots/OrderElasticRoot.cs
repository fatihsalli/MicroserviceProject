﻿using System.Text.Json;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using OrderElastic.Dtos;
using OrderElastic.Service;
using Serilog;

namespace OrderElastic.Roots;

public class OrderElasticRoot
{
    private readonly Config _config;
    private readonly KafkaConsumer _kafkaConsumer;
    private readonly OrderElasticService _orderElasticService;

    public OrderElasticRoot(Config config, KafkaConsumer kafkaConsumer, OrderElasticService orderElasticService)
    {
        _config = config;
        _kafkaConsumer = kafkaConsumer;
        _orderElasticService = orderElasticService;
    }

    public async Task StartConsumeAndSaveOrderAsync()
    {
        // Dinlemek istediğimiz topic adını belirtiyoruz.
        var topics = new List<string> { _config.Kafka.TopicName["OrderID"] };
        _kafkaConsumer.SubscribeToTopics(topics);

        while (true)
        {
            var messages = _kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 10,
                bulkConsumeMaxTimeoutInSeconds: 5, maxReadCount: 2);

            foreach (var message in messages)
            {
                var orderResponseForElastic = JsonSerializer.Deserialize<OrderResponseForElastic>(message.Value);
                var orderEventService = new OrderEventService(_config);
                var orderResponse =
                    await orderEventService.GetOrderWithHttpClientAsync(orderResponseForElastic.OrderId);
                _orderElasticService.SaveOrderToElasticsearch(orderResponse);
                Log.Information($"Received message: {message.Value}");
            }
            
            // Aynı mesajların tekrar okunmaması için message offsetlerini commitleyip temizliyoruz.
            _kafkaConsumer.CommitOffsets();
        }
    }
}