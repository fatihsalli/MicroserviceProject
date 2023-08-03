using MicroserviceProject.Services.OrderElastic.Roots;
using MicroserviceProject.Services.OrderElastic.Services;
using MicroserviceProject.Services.OrderElastic.Services.Interfaces;
using MicroserviceProject.Shared.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Konsola çıktı almak için
    .MinimumLevel.Debug() // Varsayılan log seviyesi (Debug seviyesi dahil)
    .Enrich.FromLogContext() // Ek bilgi eklemek için (opsiyonel)
    .CreateLogger();

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
            // "configs.json" dosyasını ekleyin
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string configFile = Path.Combine(projectDirectory, "configs.json");
            configBuilder.AddJsonFile(configFile, optional: false, reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            // IConfiguration servisini elde edin
            services.Configure<Config>(hostContext.Configuration.GetSection("Config"));

            services.AddScoped<IOrderElasticService, OrderElasticService>();
            services.AddScoped<IOrderEventService, OrderEventService>();
            
            services.AddScoped<OrderEventRoot>();
            services.AddScoped<OrderElasticRoot>();
            
            // DI Container'a servislerinizi ekleyin
            services.AddHostedService<Root>();
        });


// var asyncLocalLogger = new AsyncLocal<ILogger>();
//
// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console() // Konsola çıktı almak için
//     .MinimumLevel.Debug() // Varsayılan log seviyesi (Debug seviyesi dahil)
//     .Enrich.FromLogContext() // Ek bilgi eklemek için (opsiyonel)
//     .CreateLogger();
//
// Log.Information("MicroserviceProject.Services.OrderElastic Service is starting...");
//
// var orderElasticRoot = new OrderElasticRoot();
// var orderEventRoot = new OrderEventRoot();
//
// asyncLocalLogger.Value = Log.Logger;
//
// await Task.WhenAll(
//     Task.Run(async () =>
//     {
//         await orderEventRoot.StartGetOrderAndPushOrderAsync();
//     }),
//     Task.Run(async () =>
//     {
//         // Loglama context'ini yeni thread'e taşıyoruz.
//         asyncLocalLogger.Value = Log.Logger;
//         await orderElasticRoot.StartConsumeAndSaveOrderAsync();
//     })
// );
//
// Log.Information("All tasks completed. Exiting the application");
// Log.CloseAndFlush();

