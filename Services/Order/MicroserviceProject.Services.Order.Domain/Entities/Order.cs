using System.ComponentModel.DataAnnotations.Schema;
using MicroserviceProject.Services.Order.Domain.Common;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;

namespace MicroserviceProject.Services.Order.Domain.Entities;

// EF Core Features
// --Owned Types
// -- Shadow Property
// -- Backing Field
public class Order : BaseAuditableEntity
{
    public Address Address { get; set; }
    public string UserId { get; set; }

    public decimal TotalPrice { get; set; }
    
    // Kapsülleme
    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    
    private bool _done;
    
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                AddDomainEvent(new OrderCompletedEvent(this));
            }

            _done = value;
        }
    }
    
    public Order()
    {
        
    }
    
    public Order(Address address, string userId,bool done)
    {
        _orderItems = new List<OrderItem>();
        Address = address;
        UserId = userId;
        Done = done;
    }

    public void AddOrderItem(string productId, string productName, int quantity, decimal price)
    {
        var existProduct = _orderItems.Any((x => x.ProductId == productId));

        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, quantity, price);
            newOrderItem.Id = Guid.NewGuid().ToString();
            _orderItems.Add(newOrderItem);
        }
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

}