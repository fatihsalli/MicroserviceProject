﻿using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.EventHandlers;

/// <summary>
/// "OrderCreatedEventHandler" sınıfında order model başarıyla update edildikten sonra "Kafka" ya Id değerini mesaj olarak gönderiyoruz. Bu mesajı dinleyen "MicroserviceProject.Services.OrderElastic" sınıfımızda gerekli işlemler yapıldıktan sonra Elasticsearch üzerindeki kopyada update ediliyor.
/// </summary>
public class OrderUpdatedEventHandler: INotificationHandler<OrderUpdatedEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly Config _config;

    public OrderUpdatedEventHandler(KafkaProducer kafkaProducer,IOptions<Config> config)
    {
        _kafkaProducer = kafkaProducer;
        _config = config.Value;
    }
    
    public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent} - OrderId: {OrderID}", notification.GetType().Name,notification.Order.Id);
        
        // Mesajı kafkaya gönderiyoruz.
        var orderResponseForElastic = new OrderResponseForElastic
        {
            OrderId = notification.Order.Id,
            Status = "Updated"
        };

        var jsonKafkaMessage = JsonSerializer.Serialize(orderResponseForElastic);
        await _kafkaProducer.SendToKafkaWithMessageAsync(jsonKafkaMessage, _config.Kafka.TopicName["OrderID"]);
    }
}