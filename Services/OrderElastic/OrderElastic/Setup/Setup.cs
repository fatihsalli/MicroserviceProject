using System.Text.Json;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using Nest;
using OrderElastic.Service;

namespace OrderElastic.Setup;

public class Setup
{
    /// <summary>
    /// Proje dizininde yer alan "config.json" dosyasını okuyarak Shared.Config class'ına yazıyoruz. DI Container olmadığı için kendimiz yaptık.
    /// </summary>
    /// <returns></returns>
    public Config CreateConfig()
    {
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string configFile = Path.Combine(projectDirectory, "configs.json");
        string jsonConfig = File.ReadAllText(configFile);
        Config config = JsonSerializer.Deserialize<Config>(jsonConfig);
        return config;
    }

    /// <summary>
    /// Config modeli üzerinden gerekli bilgiler ile "IElasticClient" nesnesi oluşturuyoruz. Elasticservice sınıfımızda kullanmak için.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public IElasticClient CreateElasticClient(Config config)
    {
        // Elasticsearch bağlantısı ve indeksleme işlemi için yapılandırma ayarları
        var settings = new ConnectionSettings(new Uri(config.Elasticsearch.Addresses["Address-1"]))
            .DefaultIndex(config.Elasticsearch.IndexName["OrderSave"]);
        
        var client = new ElasticClient(settings);
        return client;
    }

    /// <summary>
    /// "IElasticClient" ve "Config" parametreleri ile birlikte OrderElasticService nesnemizi oluşturuyoruz.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public OrderElasticService CreateOrderElasticService(IElasticClient client,Config config)
    {
        var orderElasticService = new OrderElasticService(config,client);
        return orderElasticService;
    }
    
    /// <summary>
    /// Config modeli üzerinden gerekli bilgiler ile Shared.KafkaConsumer nesnesini oluşturuyoruz.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public KafkaConsumer CreateKafkaConsumer(Config config)
    {
        var kafkaUrl = config.Kafka.Address;
        var kafkaConsumer = new KafkaConsumer(kafkaUrl);
        return kafkaConsumer;
    }
    
    
    
}