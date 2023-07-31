using MediatR;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand: IRequest<CustomResponse<bool>>
{
    public string Id { get; set; }
    public int StatusId { get; set; }
}