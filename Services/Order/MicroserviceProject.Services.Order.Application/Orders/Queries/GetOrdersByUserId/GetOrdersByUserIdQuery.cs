using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;

public class GetOrdersByUserIdQuery : IRequest<CustomResponse<List<OrderResponse>>>
{
    public string UserId { get; set; }
}

