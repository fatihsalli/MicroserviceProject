namespace MicroserviceProject.Services.User.Application.Dtos.Responses;

public class UserWithOrdersResponse
{
    public UserWithOrdersResponse()
    {
        Orders = new List<OrderResponse>();
    }
    
    public UserResponse User { get; set; }
    public List<OrderResponse> Orders { get; set; }
}

public class OrderResponse
{
    public OrderResponse()
    {
        OrderItems = new List<OrderItemResponse>();
    }
    
    public string Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
    public AddressResponse Address { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; }
}

public class OrderItemResponse
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}