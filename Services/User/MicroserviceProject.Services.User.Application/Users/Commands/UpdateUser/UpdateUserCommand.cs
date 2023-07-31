using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Requests;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<CustomResponse<bool>>
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<AddressRequest> Addresses { get; set; }
}

