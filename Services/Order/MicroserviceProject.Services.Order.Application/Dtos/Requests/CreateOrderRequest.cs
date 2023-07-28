namespace MicroserviceProject.Services.Order.Application.Dtos.Requests;

public class CreateOrderRequest
{
    public string UserId { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
    public AddressRequest Address { get; set; }
}