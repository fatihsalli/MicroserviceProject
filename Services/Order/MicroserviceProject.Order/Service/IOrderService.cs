using MicroserviceProject.Shared.BaseService;
using OrderModel = MicroserviceProject.Shared.Models.Order;

namespace MicroserviceProject.Order.Service;

public interface IOrderService : IGenericService<OrderModel>
{
}