using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Application.Mapping;
using MicroserviceProject.Services.Order.Application.Queries;
using MicroserviceProject.Services.Order.Infrastructure;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Application.Handlers;

// Veritabanına gidip datayı alacağım sınıfım burasıdır.
public class GetOrdersByUserIdQueryHandler:IRequestHandler<GetOrdersByUserIdQuery,CustomResponse<List<OrderDto>>>
{
    // Repo olursa repoyu geçeceğiz
    private readonly OrderDbContext _context;

    public GetOrdersByUserIdQueryHandler(OrderDbContext context)
    {
        _context = context;
    }


    //GetOrdersByUserIdQuery bu sınıfı gönderdiğimizde GetOrdersByUserIdQueryHandler içindeki handle metodunu MediatR otomatik olarak bulup çalıştıracak.
    public async Task<CustomResponse<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.UserId == request.UserId)
            .ToListAsync();

        if (!orders.Any())
        {
            return CustomResponse<List<OrderDto>>.Success(200,new List<OrderDto>());
        }

        var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

        return CustomResponse<List<OrderDto>>.Success(200,ordersDto);
    }
}