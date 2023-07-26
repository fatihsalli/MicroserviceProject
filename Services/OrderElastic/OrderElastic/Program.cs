using OrderElastic.Roots;

var orderElasticRoot = new OrderElasticRoot();
var orderEventRoot = new OrderEventRoot();

await orderEventRoot.StartGetOrderAndPushOrder();
await orderElasticRoot.StartConsumeAndSaveOrderAsync();



