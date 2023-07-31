using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Requests;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CustomResponse<CreatedUserResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<AddressRequest> Addresses { get; set; }
}

