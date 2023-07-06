using MicroserviceProject.Shared.Services;
using OrderModel = MicroserviceProject.Shared.Models.Order;

namespace MicroserviceProject.Order.Service;

public interface IOrderService : IGenericService<OrderModel>
{
}