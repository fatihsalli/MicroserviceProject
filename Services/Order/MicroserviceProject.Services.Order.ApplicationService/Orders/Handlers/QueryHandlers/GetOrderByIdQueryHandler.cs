using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.QueryHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, CustomResponse<OrderResponse>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomResponse<OrderResponse>> Handle(GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await _repository.GetByIdWithIncludeAsync(request.Id, "OrderItems");

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