using MicroserviceProject.Services.OrderElastic.Roots;
using MicroserviceProject.Services.OrderElastic.Services;
using MicroserviceProject.Services.OrderElastic.Services.Interfaces;
using MicroserviceProject.Shared.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
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

