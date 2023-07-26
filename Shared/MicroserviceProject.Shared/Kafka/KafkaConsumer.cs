using Confluent.Kafka;
using Serilog;

namespace MicroserviceProject.Shared.Kafka;

/// <summary>
/// KafkaConsumer mesajları dinlediğimiz sınıftır. Constructor'dan alınan bilgiler ile birlikte "IConsumer" oluşturulur ve bunun üzerinden mesajlar tüketilir. IDisposable interface'nden miras alarak using kullandığımız senaryoda otomatik dispose edilir.
/// </summary>
public class KafkaConsumer : IDisposable
{
    private IConsumer<string, string> _consumer;
    
    // Kafka mesajlarının konumunu takip etmek için liste
    private List<TopicPartitionOffset> _messageOffsets;

    public KafkaConsumer(string kafkaUrl)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaUrl,
            GroupId = "myGroup", // Kafka tüketici grubu kimliği
            AutoOffsetReset = AutoOffsetReset.Earliest, // Konumsuz yeni bir tüketici grubuna katılınca ilk mesajlardan itibaren başla
            EnableAutoCommit = false // Otomatik ofset kayıt işlemlerini devre dışı bırakarak ofsetleri manuel olarak kaydet
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        
        // Kafka mesajlarının konumlarını tutmak için boş bir liste oluşturuyoruz
        _messageOffsets = new List<TopicPartitionOffset>();
    }

    public void SubscribeToTopics(IEnumerable<string> topics)
    {
        // Belirtilen konu veya konulara tüketiciyi abone yapıyoruz
        _consumer.Subscribe(topics);
    }
    
    /// <summary>
    /// Kafka'dan gelen mesajları tüketmemizi sağlar. 
    /// </summary>
    /// <param name="bulkConsumeIntervalInSeconds">Bu parametre, her bir toplu tüketim (bulk consume) işleminin maksimum süresini belirler. Toplu tüketim, Kafka'dan belirli bir süre boyunca birden fazla mesajı topluca tüketme işlemidir. Bu süre, kaç saniyede bir toplu tüketim yapılacağını belirler. Örneğin, bulkConsumeIntervalInSeconds = 10 ise, her 10 saniyede bir mesajları topluca tüketir.</param>
    /// <param name="bulkConsumeMaxTimeoutInSeconds">Bu parametre, toplu tüketim işleminin maksimum süresini belirler. Eğer toplu tüketim işlemi, bu süre içinde tamamlanamazsa, mevcut tüketim döngüsünden çıkılır ve diğer işlemlere geçilir. Bu, işlemin sonsuz süreyle beklemesini önler ve belirli bir zaman aşımı sınırı koyar.</param>
    /// <param name="maxReadCount">Bu parametre, her toplu tüketimde en fazla kaç mesajın okunacağını belirler. Yani, bir toplu tüketimde kaç mesajı işleyeceğinizi belirtir. Örneğin, maxReadCount = 2 ise, her toplu tüketimde en fazla 2 mesajı işleyecektir.</param>
    /// <returns></returns>
    public List<Message<string, string>> ConsumeFromTopics(int bulkConsumeIntervalInSeconds,
        int bulkConsumeMaxTimeoutInSeconds, int maxReadCount)
    {
        var messages = new List<Message<string, string>>(); // Tüketilen mesajları saklamak için liste oluşturuyoruz
        var timeoutCount = 0; // Zaman aşımı sayacı
        var start = DateTime.Now; // Zamanlayıcıyı başlatıyoruz

        while (true)
        {
            try
            {
                // Kafka'dan mesajları tüketiyoruz ve belirtilen süre içinde mesaj gelmezse beklemeyi sağlıyoruz
                var consumeResult = _consumer.Consume(new TimeSpan(0, 0, bulkConsumeMaxTimeoutInSeconds));

                if (consumeResult == null)
                {
                    // Zaman aşımına uğrayan mesajlar için sayacı artırıyoruz ve belirli bir zaman aşımı sayısına ulaşıldığında döngüden çıkıyoruz.
                    timeoutCount++;
                    if (timeoutCount > 2)
                    {
                        break;
                    }

                    continue;
                }

                if (consumeResult.Message != null)
                {
                    // Tüketilen mesajları listeye ekliyoruz
                    messages.Add(consumeResult.Message);
                }

                // Tüketilen mesajın konumunu kaydediyoruz
                _messageOffsets.Add(consumeResult.TopicPartitionOffset);

                var elapsedTime = DateTime.Now - start;
                if (elapsedTime.Seconds > bulkConsumeIntervalInSeconds || messages.Count >= maxReadCount)
                {
                    // Belirtilen süre veya maksimum mesaj sayısına ulaşıldığında döngüden çıkıyoruz
                    break;
                }
            }
            catch (ConsumeException e)
            {
                Log.Error("Kafka read messages failed. | Error: {ErrorReason}", e.Error.Reason);
                if (messages.Count > 0)
                {
                    // Eğer hata oluştuysa bile daha önce okunmuş mesajlar varsa döngüden çıkıyoruz
                    break;
                }
            }
        }

        return messages;
    }

    public void CommitOffsets()
    {
        if (_messageOffsets.Count > 0)
        {
            // Tüketilen mesajların konumlarını Kafka'ya göndererek ofsetleri kaydediyoruz
            _consumer.Commit(_messageOffsets);
            
            // Kaydedilen ofsetleri temizliyoruz
            _messageOffsets.Clear();
        }
    }

    public void Dispose()
    {
        // Nesne ömrü sona erdiğinde kalan ofsetleri kaydediyoruz
        CommitOffsets(); // Commit remaining offsets before closing
        
        // Kafka tüketici bağlantısını kapatıyoruz
        _consumer.Close();
        
        // Kafka tüketici nesnesini temizliyoruz
        _consumer.Dispose();
    }
}