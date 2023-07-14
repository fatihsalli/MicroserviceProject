using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;

public class GetAllOrdersQuery : IRequest<CustomResponse<List<OrderResponse>>>
{
    
}

public class GetOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery,CustomResponse<List<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<List<OrderResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        //OrderItemsları da dahil ettik.
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .ToListAsync();

        //Maplemeden önce dolu mu boş mu diye kontrol ediyoruz. Yoksa Automapper kullanırken hata alırız.
        if (!orders.Any())
        {
            return CustomResponse<List<OrderResponse>>.Success(200,new List<OrderResponse>());
        }

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

        return CustomResponse<List<OrderResponse>>.Success(200,orderResponses);
    }
}