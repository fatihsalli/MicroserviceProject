namespace MicroserviceProject.Shared.Models.Responses;

public class UserWithOrdersResponse
{
    public UserWithOrdersResponse()
    {
        Orders = new List<OrderResponse>();
    }
    
    public UserResponse User { get; set; }
    public List<OrderResponse> Orders { get; set; }
}