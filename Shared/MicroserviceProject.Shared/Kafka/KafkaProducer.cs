using Confluent.Kafka;
using Serilog;

namespace MicroserviceProject.Shared.Kafka;

public class KafkaProducer
{
    private readonly IProducer<string, string> _producer;
    
    public KafkaProducer(string kafkaHost)
    {
        var config = new ProducerConfig { BootstrapServers = kafkaHost };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public void SendToKafkaWithMessage(string message, string topic)
    {
        try
        {
            var deliveryReport = _producer.ProduceAsync(topic, new Message<string, string> { Value = message });
            deliveryReport.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null) Log.Error("Delivery failed: {ErrorReason}", task.Exception.Message);
                }
                else
                {
                    Log.Information("Delivered message to: {ResultTopicPartitionOffset}", task.Result.TopicPartitionOffset);
                }
            });
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