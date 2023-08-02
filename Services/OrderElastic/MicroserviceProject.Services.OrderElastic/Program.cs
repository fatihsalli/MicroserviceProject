using MicroserviceProject.Services.OrderElastic.Roots;
using Serilog;

var asyncLocalLogger = new AsyncLocal<ILogger>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Konsola çıktı almak için
    .MinimumLevel.Debug() // Varsayılan log seviyesi (Debug seviyesi dahil)
    .Enrich.FromLogContext() // Ek bilgi eklemek için (opsiyonel)
    .CreateLogger();

Log.Information("MicroserviceProject.Services.OrderElastic Service is starting...");

var orderElasticRoot = new OrderElasticRoot();
var orderEventRoot = new OrderEventRoot();

asyncLocalLogger.Value = Log.Logger;

await Task.WhenAll(
    Task.Run(async () =>
    {
        await orderEventRoot.StartGetOrderAndPushOrderAsync();
    }),
    Task.Run(async () =>
    {
        // Loglama context'ini yeni thread'e taşıyoruz.
        asyncLocalLogger.Value = Log.Logger;
        await orderElasticRoot.StartConsumeAndSaveOrderAsync();
    })
);

Log.Information("All tasks completed. Exiting the application");
Log.CloseAndFlush();

