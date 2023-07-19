using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Infrastructure.Persistence;
using MicroserviceProject.Services.User.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroserviceProject.Services.User.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Options Pattern
        services.Configure<UserDatabaseSettings>(configuration.GetSection(nameof(UserDatabaseSettings)));
        services.AddSingleton<IUserDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);
        
        //Database
        services.AddScoped<IUserDbContext, UserDbContext>();
        
        return services;
    }
}