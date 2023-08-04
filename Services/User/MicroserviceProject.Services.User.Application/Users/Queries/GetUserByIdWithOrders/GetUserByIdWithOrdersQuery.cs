using MediatR;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetUserByIdWithOrders;

public record GetUserByIdWithOrdersQuery(string Id) : IRequest<CustomResponse<UserWithOrdersResponse>>;

