namespace MicroserviceProject.Shared.Models;

public class Order : BaseModel
{
    public string UserId { get; set; }
    public string Status { get; set; }
    public Address Address { get; set; }
    public Address InvoiceAddress { get; set; }
    public decimal TotalPrice { get; set; }
    public List<Product> Products { get; set; }
}

public class Product
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
