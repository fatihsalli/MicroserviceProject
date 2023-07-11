using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Queries;

// IRequest ile MediatR kullanarak işaretledik CustomResponse ile de response u belirtiyoruz.
public class GetOrdersByUserIdQuery : IRequest<CustomResponse<List<OrderDto>>>
{
    public string UserId { get; set; }
}