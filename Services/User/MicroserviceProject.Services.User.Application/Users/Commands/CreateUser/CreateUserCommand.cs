using MediatR;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Requests;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CustomResponse<UserCreatedResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<AddressRequest> Addresses { get; set; }
}

