using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(string Id) : IRequest<CustomResponse<OrderResponse>>
{
    
}

