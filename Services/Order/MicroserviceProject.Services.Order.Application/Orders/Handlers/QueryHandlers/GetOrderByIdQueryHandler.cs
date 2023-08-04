using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, CustomResponse<OrderResponse>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<OrderResponse>> Handle(GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            var orderResponse = _mapper.Map<OrderResponse>(order);

            return CustomResponse<OrderResponse>.Success(200, orderResponse);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "GetOrderByIdQueryHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "GetOrderByIdQueryHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}