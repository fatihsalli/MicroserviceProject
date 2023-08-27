using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Mappings;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;
using MicroserviceProject.Services.Order.Repository;
using MicroserviceProject.Services.Order.Repository.Interfaces;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, CustomResponse<PaginatedList<OrderResponse>>>
{
    private readonly OrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByUserIdQueryHandler(OrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<PaginatedList<OrderResponse>>> Handle(GetOrdersByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                .PaginatedAllListAsync();

            return CustomResponse<PaginatedList<OrderResponse>>.Success(200, orders);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrderByUserIdQueryHandler Exception");
            throw new Exception("Something went wrong!");
        }
    }
}