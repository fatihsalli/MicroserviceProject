using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CustomResponse<CreatedOrderResponse>>
{
    public string UserId { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
    public AddressRequest Address { get; set; }
}