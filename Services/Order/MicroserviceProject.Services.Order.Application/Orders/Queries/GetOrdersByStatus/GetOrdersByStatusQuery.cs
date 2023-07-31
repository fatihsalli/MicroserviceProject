using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;

public class GetOrdersByStatusQuery: IRequest<CustomResponse<List<OrderResponse>>>
{
    public int StatusId { get; set; }
}