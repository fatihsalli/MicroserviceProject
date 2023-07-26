// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Text.Json;
using MicroserviceProject.Shared.Responses;
using OrderElastic.Dtos;
using OrderElastic.Setup;

var setup = new Setup();
var config = setup.CreateConfig();
var elasticClient = setup.CreateElasticClient(config);
var orderElasticService = setup.CreateOrderElasticService(elasticClient, config);
var kafkaConsumer = setup.CreateKafkaConsumer(config);

while (true)
{
    var topics = new List<string> { config.Kafka.TopicName["OrderID"] }; // Dinlemek istediğiniz topic adını belirtin
    
    kafkaConsumer.SubscribeToTopics(topics);

    var messages = kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 30, bulkConsumeMaxTimeoutInSeconds: 5, maxReadCount: 10);
    
    foreach (var message in messages)
    {
        var orderResponseForElastic = JsonSerializer.Deserialize<OrderResponseForElastic>(message.Value);

        var httpClient = new HttpClient();
        
        // Get Orders with HttpClient
        string requestUrl = $"http://localhost:5011/api/Orders/{orderResponseForElastic.OrderId}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            var responseFail = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>();
            throw new Exception($"Errors:{responseFail.Errors} - StatusCode:{responseFail.StatusCode}");
        }
            
        var responseSuccess = await response.Content.ReadFromJsonAsync<CustomResponse<OrderResponse>>();
        
        orderElasticService.SaveOrderToElasticsearch(responseSuccess.Data);
        
        Console.WriteLine($"Received message: {message.Value}");
    }

    kafkaConsumer.CommitOffsets();
}