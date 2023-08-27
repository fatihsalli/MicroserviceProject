using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;

public class GetOrdersByUserIdQuery : IRequest<CustomResponse<PaginatedList<OrderResponse>>>
{
    public string UserId { get; set; }
}

