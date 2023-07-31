using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Common.Mappings;
using MicroserviceProject.Services.Order.Application.Common.Models;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersWithPagination;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrdersWithPaginationQueryHandler : IRequestHandler
    <GetOrdersWithPaginationQuery, CustomResponse<PaginatedList<OrderResponse>>>
{
    private readonly IOrderDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersWithPaginationQueryHandler(IOrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<CustomResponse<PaginatedList<OrderResponse>>> Handle(GetOrdersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // ProjectTo ile hem mapleme işlemini yapıyoruz hem de veritabanından "OrderResponse" modele property lerine göre veri çekiyoruz.(?)
        var response= await _context.Orders
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return CustomResponse<PaginatedList<OrderResponse>>.Success(200, response);
    }
}