using MediatR;
using MicroserviceProject.Shared.Models.Responses;
using MicroserviceProject.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;
using MicroserviceProject.Shared.Exceptions;
using Serilog;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MicroserviceProject.Shared.Enums;

namespace MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderTest
{
    public class GetOrderTestQuery : IRequest<CustomResponse<OrderCustomResponse>>
    {
        public string Id { get; set; }
    }

    public class GetOrderTestHandler : IRequestHandler<GetOrderTestQuery, CustomResponse<OrderCustomResponse>>
    {
        private readonly IOrderDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderTestHandler(IOrderDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomResponse<OrderCustomResponse>> Handle(GetOrderTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Sadece single olursa null olması durumunda exception fırlatır. "FirstOrDefault" veya "SingleOrDefault" da bu sorun olmaz.
                var order = await _context.Orders
                    .Where(o => o.Id == request.Id)
                    .Select(o => new { o.Id, o.TotalPrice })
                    .SingleOrDefaultAsync(cancellationToken);


                var orders = _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.StatusId == (OrderStatus)1);


                var orders2 = await _context.Orders.ToListAsync();

                var orderIDs=orders2.Select(o=> o.Id);



                if (order == null)
                    throw new NotFoundException("order", request.Id);

                var orderResponse = new OrderCustomResponse { Id = order.Id, TotalPrice = order.TotalPrice };

                return CustomResponse<OrderCustomResponse>.Success(200, orderResponse);
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

}
