using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Domain.OrderAggregate;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Commands;

public class CreateOrderCommand:IRequest<CustomResponse<CreatedOrderDto>>
{
    public string UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public AddressDto Address { get; set; }
}