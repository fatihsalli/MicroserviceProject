using Confluent.Kafka;
using Serilog;

namespace MicroserviceProject.Shared.Kafka;

public class KafkaConsumer : IDisposable
{
    private IConsumer<string, string> consumer;
    private List<TopicPartitionOffset> messageOffsets;

    public KafkaConsumer(string kafkaURL, string groupId, int bulkConsumeMaxTimeoutInSeconds)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaURL,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false // Disable auto commit to manually commit offsets
        };

        consumer = new ConsumerBuilder<string, string>(config).Build();
        messageOffsets = new List<TopicPartitionOffset>();
    }

    public void SubscribeToTopics(IEnumerable<string> topics)
    {
        consumer.Subscribe(topics);
    }

    public List<Message<string, string>> ConsumeFromTopics(int bulkConsumeIntervalInSeconds,
        int bulkConsumeMaxTimeoutInSeconds, int maxReadCount)
    {
        var messages = new List<Message<string, string>>();
        var timeoutCount = 0;
        var start = DateTime.Now;

        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume(new TimeSpan(0, 0, bulkConsumeMaxTimeoutInSeconds));

                if (consumeResult == null)
                {
                    timeoutCount++;
                    if (timeoutCount > 2)
                    {
                        break;
                    }

                    continue;
                }

                if (consumeResult.Message != null)
                {
                    messages.Add(consumeResult.Message);
                }

                messageOffsets.Add(consumeResult.TopicPartitionOffset);

                var elapsedTime = DateTime.Now - start;
                if (elapsedTime.Seconds > bulkConsumeIntervalInSeconds || messages.Count >= maxReadCount)
                {
                    break;
                }
            }
            catch (ConsumeException e)
            {
                Log.Error("Kafka read messages failed. | Error: {ErrorReason}", e.Error.Reason);
                if (messages.Count > 0)
                {
                    break;
                }
            }
        }

        return messages;
    }

    public void CommitOffsets()
    {
        if (messageOffsets.Count > 0)
        {
            consumer.Commit(messageOffsets);
            messageOffsets.Clear();
        }
    }

    public void Dispose()
    {
        CommitOffsets(); // Commit remaining offsets before closing
        consumer.Close();
        consumer.Dispose();
    }
}