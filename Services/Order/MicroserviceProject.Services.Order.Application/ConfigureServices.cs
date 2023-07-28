using System.Reflection;
using FluentValidation;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Behaviours;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroserviceProject.Services.Order.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Add FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Add KafkaProducer
        services.AddScoped<KafkaProducer>(provider =>
        {
            var config = provider.GetService<IOptions<Config>>().Value;
            return new KafkaProducer(config.Kafka.Address);
        });

        // Add HttpClient
        services.AddHttpClient();

        // Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}