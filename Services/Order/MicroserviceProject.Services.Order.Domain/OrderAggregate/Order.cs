using MicroserviceProject.Services.Order.Domain.Core;

namespace MicroserviceProject.Services.Order.Domain.OrderAggregate;

// EF Core Features
// --Owned Types
// -- Shadow Property
// -- Backing Field
public class Order : Entity, IAggregateRoot
{
    public DateTime CreatedAt { get; set; }
    public Address Address { get; set; }
    public string UserId { get; set; }
    private readonly List<OrderItem> _orderItems;

    // Kapsülleme
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order(Address address, string userId)
    {
        _orderItems = new List<OrderItem>();
        CreatedAt=DateTime.Now;
        Address = address;
        UserId = userId;
    }

    public void AddOrderItem(string productId, string productName, int quantity, decimal price)
    {
        var existProduct = _orderItems.Any((x => x.ProductId == productId));

        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, quantity, price);
            _orderItems.Add(newOrderItem);
        }
    }

    public decimal GetTotalPrice => _orderItems.Sum(x => x.Price * x.Quantity);

}