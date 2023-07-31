using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<CustomResponse<UserResponse>>;

