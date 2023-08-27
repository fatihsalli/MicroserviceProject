using Autofac;
using MicroserviceProject.Services.Order.Application.Helpers;
using MicroserviceProject.Services.Order.Container.Modules;
using MicroserviceProject.Services.Order.Repository.Interceptors;

namespace MicroserviceProject.Services.Order.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new MediatRModule());
        containerBuilder.RegisterModule(new RepositoryModule());
        containerBuilder.RegisterModule(new RmqBusModule());
        
        containerBuilder.RegisterType<AuditableEntitySaveChangesInterceptor>().InstancePerLifetimeScope();
        
        containerBuilder.RegisterType<CorrelationIdGenerator>().As<ICorrelationIdGenerator>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope autofacContainer)
    {
        Container = autofacContainer;
    }
}