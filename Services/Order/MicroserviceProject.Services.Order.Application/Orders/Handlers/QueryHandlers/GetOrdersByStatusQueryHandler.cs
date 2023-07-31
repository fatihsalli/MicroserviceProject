using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;
using MicroserviceProject.Shared.Enums;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrdersByStatusQueryHandler: IRequestHandler<GetOrdersByStatusQuery, CustomResponse<List<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByStatusQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<CustomResponse<List<OrderResponse>>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Where(x => x.StatusId == (OrderStatus)request.StatusId)
                .ToListAsync(cancellationToken);

            var ordersResponse = _mapper.Map<List<OrderResponse>>(orders);

            return CustomResponse<List<OrderResponse>>.Success(200, ordersResponse,ordersResponse.Count);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrdersByStatusQueryHandler Exception");
            throw new Exception("Something went wrong!");
        }
    }
}