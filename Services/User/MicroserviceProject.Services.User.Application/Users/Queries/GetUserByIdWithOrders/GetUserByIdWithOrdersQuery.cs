using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetUserByIdWithOrders;

public record GetUserByIdWithOrdersQuery(string Id) : IRequest<CustomResponse<UserWithOrdersResponse>>;

