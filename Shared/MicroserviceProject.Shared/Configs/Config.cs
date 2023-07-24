namespace MicroserviceProject.Shared.Configs;

public class Config
{
    public ElasticsearchConfig Elasticsearch { get; set; }
    public KafkaConfig Kafka { get; set; }
    public HttpClientConfig HttpClient { get; set; }
}

public class ElasticsearchConfig
{
    public Dictionary<string, string> Addresses { get; set; }
    public Dictionary<string, string> IndexName { get; set; }
}

public class KafkaConfig
{
    public string Address { get; set; }
    public Dictionary<string, string> TopicName { get; set; }
}

public class HttpClientConfig
{
    public string UserApi { get; set; }
    public string OrderApi { get; set; }
}

