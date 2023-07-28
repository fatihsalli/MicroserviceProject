using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Infrastructure.Persistence;
using MicroserviceProject.Services.Order.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Order.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<OrderDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                configure => { configure.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName); });
        });

        // Direkt olarak OrderDbContext'i de geçebilirdik ancak proje referans yapılanmasında Infrastructure=>Application katmanını kullandığı için interface üzerinden erişebilecek.
        services.AddScoped<IOrderDbContext>(provider => provider.GetRequiredService<OrderDbContext>());

        return services;
    }
}