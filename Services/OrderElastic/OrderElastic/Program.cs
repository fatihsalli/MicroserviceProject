using OrderElastic.Roots;
using OrderElastic.Setup;

var setup = new Setup();
var config = setup.CreateConfig();
var elasticClient = setup.CreateElasticClient(config);
var orderElasticService = setup.CreateOrderElasticService(elasticClient, config);

var kafkaConsumerElasticRoot = setup.CreateKafkaConsumer(config);
var orderElasticRoot = new OrderElasticRoot(config, kafkaConsumerElasticRoot, orderElasticService);

await orderElasticRoot.StartConsumeAndSaveOrderAsync();


