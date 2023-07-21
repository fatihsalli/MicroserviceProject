using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;

public class GetOrdersByUserIdQuery : IRequest<CustomResponse<List<OrderResponse>>>
{
    public string UserId { get; set; }
}

public class
    GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, CustomResponse<List<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByUserIdQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<List<OrderResponse>>> Handle(GetOrdersByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            var ordersResponse = _mapper.Map<List<OrderResponse>>(orders);

            return CustomResponse<List<OrderResponse>>.Success(200, ordersResponse);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetOrderByUserIdQueryHandler Exception");
            throw new Exception($"Something went wrong!");
        }
    }
}