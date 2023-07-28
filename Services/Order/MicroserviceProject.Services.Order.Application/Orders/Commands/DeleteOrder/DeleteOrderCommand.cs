using MediatR;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(string Id) : IRequest<CustomResponse<bool>>;