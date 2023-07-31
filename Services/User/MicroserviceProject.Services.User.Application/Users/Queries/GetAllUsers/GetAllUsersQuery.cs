using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<CustomResponse<List<UserResponse>>>
{
    
}

