using MicroserviceProject.Services.Order.Domain.Common;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Enums;

namespace MicroserviceProject.Services.Order.Domain.Entities;

// EF Core Features
// -- Owned Types
// -- Shadow Property
// -- Backing Field
public class Order : BaseAuditableEntity
{
    public string UserId { get; set; }
    public decimal TotalPrice{ get; set; }

    //"Owned Entity Type" biz bunu tanımladığımızda EF Core'a müdahale etmez isek Order içinde Address ile ilgili sütunları oluşturur.
    public Address Address { get; set; }
    public OrderStatus StatusId { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }

    //Backing fields. Order üzerinden kontrolsüz şekilde kimse orderItems a data eklememesi için oluşturduk. EF core dolduracak bir alt satırda da readonly olarak dış dünyaya açacağız.
    private readonly List<OrderItem> _orderItems;

    //Kapsülleme işlemi yaptık. Sadece okunması için.
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    private bool _done;

    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                AddDomainEvent(new OrderCompletedEvent(this));
            }

            _done = value;
        }
    }

    public Order()
    {
        _orderItems = new List<OrderItem>();
    }

    public void AddOrderItem(string productId, string productName, int quantity, decimal price)
    {
        var existProduct = _orderItems.Any(x => x.ProductId == productId);

        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, quantity, price)
            {
                Id = Guid.NewGuid().ToString()
            };
            _orderItems.Add(newOrderItem);
        }
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }
}