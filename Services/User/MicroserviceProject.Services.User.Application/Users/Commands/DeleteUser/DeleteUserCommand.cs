using MediatR;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<CustomResponse<bool>>;

