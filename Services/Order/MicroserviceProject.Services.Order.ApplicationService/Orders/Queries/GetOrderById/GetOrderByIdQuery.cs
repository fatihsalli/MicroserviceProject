using AutoMapper;
using MediatR;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(string Id) : IRequest<CustomResponse<OrderResponse>>
{
    
}

