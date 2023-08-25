using Autofac;

namespace MicroserviceProject.Services.Order.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder containerBuilder)
    {
        
    }

    public static void SetContainer(ILifetimeScope autofacContainer)
    {
        Container = autofacContainer;
    }
}