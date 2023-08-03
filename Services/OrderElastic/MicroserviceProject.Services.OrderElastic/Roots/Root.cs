using Microsoft.Extensions.Hosting;
using Serilog;

namespace MicroserviceProject.Services.OrderElastic.Roots;

public class Root : BackgroundService
{
    private readonly OrderElasticRoot _orderElasticRoot;
    private readonly OrderEventRoot _orderEventRoot;

    public Root(OrderElasticRoot orderElasticRoot, OrderEventRoot orderEventRoot)
    {
        _orderElasticRoot = orderElasticRoot;
        _orderEventRoot = orderEventRoot;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var asyncLocalLogger = new AsyncLocal<ILogger>
        {
            Value = Log.Logger
        };

        await Task.WhenAll(
            Task.Run(async () =>
            {
                await _orderEventRoot.StartGetOrderAndPushOrderAsync();
            }, stoppingToken),
            Task.Run(async () =>
            {
                // Loglama context'ini yeni thread'e taşıyoruz.
                asyncLocalLogger.Value = Log.Logger;
                await _orderElasticRoot.StartConsumeAndSaveOrderAsync();
            }, stoppingToken)
        );
    }
}