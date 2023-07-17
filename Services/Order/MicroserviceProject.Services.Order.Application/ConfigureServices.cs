using System.Reflection;
using FluentValidation;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Behaviours;
using MicroserviceProject.Services.Order.Application.Mapping;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Order.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(MapperProfile));
        
        // Add FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Add MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }
}