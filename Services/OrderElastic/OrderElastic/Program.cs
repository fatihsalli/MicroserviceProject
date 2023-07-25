// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Text.Json;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using OrderElastic.Dtos;
using OrderElastic.Service;

while (true)
{
    var orderElasticService = new OrderElasticService();
    
    
    // Geçici => Datayı okuma
    var kafkaURL = "localhost:9092"; // Kafka broker'ınıza göre değiştirin
    var groupId = "myGroup"; // Tüketici grubu adını belirtin
    var bulkConsumeMaxTimeoutInSeconds = 5; // Maksimum zaman aşımı süresini belirtin

    var kafkaConsumer = new KafkaConsumer(kafkaURL, groupId, bulkConsumeMaxTimeoutInSeconds);
    
    var topics = new List<string> { "orderID-created-v01" }; // Dinlemek istediğiniz topic adını belirtin
    kafkaConsumer.SubscribeToTopics(topics);

    var messages = kafkaConsumer.ConsumeFromTopics(bulkConsumeIntervalInSeconds: 30, bulkConsumeMaxTimeoutInSeconds: bulkConsumeMaxTimeoutInSeconds, maxReadCount: 10);
    
    foreach (var message in messages)
    {
        var orderResponseForElastic = JsonSerializer.Deserialize<OrderResponseForElastic>(message.Value);

        var client = new HttpClient();
        
        // Get Orders with HttpClient
        string requestUrl = $"http://localhost:5011/api/Orders/{orderResponseForElastic.OrderId}";
        HttpResponseMessage response = await client.GetAsync(requestUrl);

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