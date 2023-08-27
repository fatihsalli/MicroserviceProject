using Autofac;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Order.Container.Modules;

public class RmqBusModule : Module
{
    private static string RmqUri { get; set; }
    private static string RmqUser { get; set; }
    private static string RmqPassword { get; set; }
    private static string RmqClusters { get; set; }

    public static void SetProperties(IConfiguration configuration)
    {
        RmqUri = configuration["RMQUri"];
        RmqUser = configuration["RMQUser"];
        RmqPassword = configuration["RMQPassword"];
        RmqClusters = configuration["RmqClusters"];
    }

    protected override void Load(ContainerBuilder builder)
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(new Uri(RmqUri), h =>
            {
                h.Username(RmqUser);
                h.Password(RmqPassword);
                h.UseCluster(c =>
                {
                    foreach (var node in RmqClusters.Split(';'))
                    {
                        c.Node(node);
                    }
                });
            });
        });

        builder
            .Register(context => busControl)
            .SingleInstance()
            .As<IBus>()
            .As<IBusControl>();
    }
}