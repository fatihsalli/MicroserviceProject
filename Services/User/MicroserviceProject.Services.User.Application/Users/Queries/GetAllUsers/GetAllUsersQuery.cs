using MediatR;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<CustomResponse<List<UserResponse>>>
{
    
}

