using MediatR;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<CustomResponse<UserResponse>>;

