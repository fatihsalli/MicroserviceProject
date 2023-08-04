namespace MicroserviceProject.Shared.Models.Requests;

public class CreateOrderRequest
{
    public string UserId { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
    public AddressRequest Address { get; set; }
}