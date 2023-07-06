using MicroserviceProject.Order.Service;
using MicroserviceProject.Shared.BaseService;

namespace MicroserviceProject.Order.Extensions;

public static class ServiceExtension
{
    public static void AddServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
    }
}