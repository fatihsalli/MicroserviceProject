namespace MicroserviceProject.Services.Order.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public AddressDto Address { get; set; }
    public string UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }

}