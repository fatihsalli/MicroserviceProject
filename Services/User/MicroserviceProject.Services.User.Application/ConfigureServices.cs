using System.Reflection;
using FluentValidation;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.User.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Add FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add HttpClient
        services.AddHttpClient();
        
        // Add MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }
}