using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;
using MicroserviceProject.Services.Order.Application.Common.Mappings;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

/// <summary>
/// Tüm order modellerimi response olarak döndüğüm metottur. "PaginatedAllListAsync" kullanmamızın nedeni sayfalama yapmadan "PaginatedList" içerisindeki count vb. değerleri kullanmak içindir.
/// </summary>
public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, CustomResponse<PaginatedList<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<PaginatedList<OrderResponse>>> Handle(GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                .PaginatedAllListAsync();

            return CustomResponse<PaginatedList<OrderResponse>>.Success(200, orders);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrdersQueryHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}