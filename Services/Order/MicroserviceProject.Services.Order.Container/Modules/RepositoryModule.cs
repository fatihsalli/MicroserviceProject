using System.Reflection;
using Autofac;
using MicroserviceProject.Services.Order.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;

namespace MicroserviceProject.Services.Order.Container.Modules;

public class RepositoryModule : Module
{
    private static string _connectionString;

    public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        _connectionString = configuration["DbConnString"];

        serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<OrderDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(_connectionString);
        });
    }

    protected override void Load(ContainerBuilder builder)
    {
        var assemblyType = typeof(OrderRepository).GetTypeInfo();

        builder.RegisterAssemblyTypes(assemblyType.Assembly)
            .Where(x => x != typeof(OrderDbContext))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        base.Load(builder);
    }
}