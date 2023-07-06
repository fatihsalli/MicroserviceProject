namespace MicroserviceProject.Order.Service;

public interface IOrderService
{
    public List<Shared.Models.Order> GetAll();
    public Shared.Models.Order Create(Shared.Models.Order order);
}