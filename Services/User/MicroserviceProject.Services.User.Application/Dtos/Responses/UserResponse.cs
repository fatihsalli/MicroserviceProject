namespace MicroserviceProject.Services.User.Application.Dtos.Responses;

public class UserResponse
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<AddressResponse> Addresses { get; set; }
}