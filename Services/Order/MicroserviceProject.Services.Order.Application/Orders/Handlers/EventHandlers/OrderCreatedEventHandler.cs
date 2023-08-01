using System.Text.Json;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.EventHandlers;

/// <summary>
/// "OrderCreatedEventHandler" sınıfında order model başarıyla kaydedildikten sonra "Kafka" ya Id değerini mesaj olarak gönderiyoruz. Bu mesajı dinleyen "OrderElastic" sınıfımızda gerekli işlemler yapıldıktan sonra Elasticsearch üzerine bir kopyası kaydediliyor.
/// </summary>
public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly Config _config;

    public OrderCreatedEventHandler(KafkaProducer kafkaProducer,IOptions<Config> config)
    {
        _kafkaProducer = kafkaProducer;
        _config = config.Value;
    }
    
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent} - OrderId: {OrderID}", notification.GetType().Name,notification.Order.Id);
        
        // Mesajı kafkaya gönderiyoruz.
        var orderResponseForElastic = new OrderResponseForElastic
        {
            OrderId = notification.Order.Id,
            Status = "Created"
        };

        var jsonKafkaMessage = JsonSerializer.Serialize(orderResponseForElastic);
        await _kafkaProducer.SendToKafkaWithMessageAsync(jsonKafkaMessage, _config.Kafka.TopicName["OrderID"]);
    }
}