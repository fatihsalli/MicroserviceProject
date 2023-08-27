using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Mappings;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;
using MicroserviceProject.Services.Order.Repository;
using MicroserviceProject.Services.Order.Repository.Interfaces;
using MicroserviceProject.Shared.Enums;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrdersByStatusQueryHandler: IRequestHandler<GetOrdersByStatusQuery, CustomResponse<PaginatedList<OrderResponse>>>
{
    private readonly OrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByStatusQueryHandler(OrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<CustomResponse<PaginatedList<OrderResponse>>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Where(x => x.StatusId == (OrderStatus)request.StatusId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                .PaginatedAllListAsync();

            return CustomResponse<PaginatedList<OrderResponse>>.Success(200, orders);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrdersByStatusQueryHandler Exception");
            throw new Exception("Something went wrong!");
        }
    }
}