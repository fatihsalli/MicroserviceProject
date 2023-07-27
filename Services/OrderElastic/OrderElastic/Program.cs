using OrderElastic.Roots;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Konsola çıktı almak için
    .MinimumLevel.Debug() // Varsayılan log seviyesi (Debug seviyesi dahil)
    .Enrich.FromLogContext() // Ek bilgi eklemek için (opsiyonel)
    .CreateLogger();

Log.Information("OrderElastic Service is starting...");

var orderElasticRoot = new OrderElasticRoot();
var orderEventRoot = new OrderEventRoot();

await orderEventRoot.StartGetOrderAndPushOrderAsync();
await orderElasticRoot.StartConsumeAndSaveOrderAsync();

