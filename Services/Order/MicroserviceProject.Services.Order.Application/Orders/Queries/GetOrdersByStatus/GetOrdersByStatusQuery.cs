using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;

public class GetOrdersByStatusQuery: IRequest<CustomResponse<PaginatedList<OrderResponse>>>
{
    public int StatusId { get; set; }
}