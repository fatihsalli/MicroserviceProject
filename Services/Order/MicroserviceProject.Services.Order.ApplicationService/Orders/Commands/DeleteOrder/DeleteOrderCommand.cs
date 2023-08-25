using MediatR;
using MicroserviceProject.Shared.Models;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(string Id) : IRequest<CustomResponse<bool>>;