using OrderElastic.Roots;
using OrderElastic.Setup;

var setup = new Setup();
var config = setup.CreateConfig();
var elasticClient = setup.CreateElasticClient(config);
var orderElasticService = setup.CreateOrderElasticService(elasticClient, config);
var kafkaConsumer = setup.CreateKafkaConsumer(config);
var orderElasticRoot = new OrderElasticRoot(config, kafkaConsumer, orderElasticService);

await orderElasticRoot.StartConsumeAndSaveOrderAsync();


