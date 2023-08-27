using Autofac;
using MicroserviceProject.Services.Order.Application.Helpers;
using MicroserviceProject.Services.Order.Container.Modules;

namespace MicroserviceProject.Services.Order.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new MediatRModule());
        
        containerBuilder.RegisterType<CorrelationIdGenerator>().As<ICorrelationIdGenerator>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope autofacContainer)
    {
        Container = autofacContainer;
    }
}