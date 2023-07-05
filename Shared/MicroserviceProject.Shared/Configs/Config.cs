namespace MicroserviceProject.Shared.Configs;

public class Config
{
    public ServerConfig Server { get; set; }
    public DatabaseConfig Database { get; set; }
    public ElasticsearchConfig Elasticsearch { get; set; }
    public KafkaConfig Kafka { get; set; }
    public HttpClientConfig HttpClient { get; set; }
}

public class ServerConfig
{
    public Dictionary<string, string> Port { get; set; }
    public string Host { get; set; }
}

public class DatabaseConfig
{
    public string Connection { get; set; }
    public string DatabaseName { get; set; }
    public string UserCollectionName { get; set; }
    public string OrderCollectionName { get; set; }
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
    public string UserAPI { get; set; }
    public string OrderAPI { get; set; }
}

