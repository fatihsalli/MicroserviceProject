﻿using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, CustomResponse<List<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<List<OrderResponse>>> Handle(GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .ToListAsync();

            //Maplemeden önce dolu mu boş mu diye kontrol ediyoruz. Yoksa Automapper kullanırken hata alırız.
            if (!orders.Any())
                return CustomResponse<List<OrderResponse>>.Success(200, new List<OrderResponse>());

            var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

            return CustomResponse<List<OrderResponse>>.Success(200, orderResponses);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrdersQueryHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}