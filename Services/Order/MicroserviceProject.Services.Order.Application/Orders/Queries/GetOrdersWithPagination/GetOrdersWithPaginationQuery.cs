using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersWithPagination;

public class GetOrdersWithPaginationQuery : IRequest<CustomResponse<PaginatedList<OrderResponse>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}