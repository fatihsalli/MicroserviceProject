using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrderStatus;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Enums;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class UpdateOrderStatusCommandHandler: IRequestHandler<UpdateOrderStatusCommand, CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;

    public UpdateOrderStatusCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<CustomResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            order.StatusId = (OrderStatus)request.StatusId;
            order.Status = OrderStatusHelper.OrderStatusString[(int)order.StatusId];
            order.Description = OrderStatusHelper.OrderStatusDescriptions[(int)order.StatusId];

            // Done=true olduğunda otomatik olarak OrderCompletedEvent fırlatılıyor.
            if (order.StatusId==OrderStatus.Completed)
            {
                order.Done = true;
            }
            else
            {
                order.AddDomainEvent(new OrderUpdatedEvent(order));
            }

            await _context.SaveChangesAsync(cancellationToken);
            await _context.PublishDomainEvents();

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "UpdateOrderCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "UpdateOrderCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}