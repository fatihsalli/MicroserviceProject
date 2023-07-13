using MicroserviceProject.Services.Order.Domain.Common;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;

namespace MicroserviceProject.Services.Order.Domain.Entities;

// EF Core Features
// --Owned Types
// -- Shadow Property
// -- Backing Field
public class Order : AuditableEntity, IHasDomainEvent
{
    public Address Address { get; set; }
    public string UserId { get; set; }
    private readonly List<OrderItem> _orderItems;

    // Kapsülleme
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    
    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                DomainEvents.Add(new OrderCompletedEvent(this));
            }

            _done = value;
        }
    }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    
    

    public Order()
    {
        
    }
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