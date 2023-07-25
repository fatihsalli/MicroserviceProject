using Confluent.Kafka;
using Serilog;

namespace MicroserviceProject.Shared.Kafka;

/// <summary>
/// KafkaProducer sınıfı "Confluent.Kafka" paketini kullanarak Kafka'ya mesaj gönderebilmemizi sağlar. Constructor üzerinden üretilen "Producer" üzerinden "SendToKafkaWithMessage" metodu ile mesaj, topic ile birlikte gönderilir. "IDisposable" sınıfından miras alındı using ile kullandığımızda işimiz bittiğinde otomatik dispose edebilmek için.
/// </summary>
public class KafkaProducer : IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(string kafkaHost)
    {
        var config = new ProducerConfig { BootstrapServers = kafkaHost };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendToKafkaWithMessage(string message, string topic)
    {
        try
        {
            // "ProduceAsync" metodu ile asenkron olarak mesaj Kafka'ya gönderilir. Hata olması durumunda "ProduceException" türünde bir hata fırlatacaktır. Onu catch bloğunda yakalayabiliriz o sebeple burada "DeliveryResult" status değerini kontrol etmek gereksiz olacaktır. 
            var deliveryReport = await _producer.ProduceAsync(topic, new Message<string, string> { Value = message });

            Log.Information("Message delivered to Kafka topic: {Topic}", deliveryReport.Topic);
        }
        catch (ProduceException<string, string> e)
        {
            Log.Error("Delivery failed: {ErrorReason}", e.Error.Reason);
        }
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}