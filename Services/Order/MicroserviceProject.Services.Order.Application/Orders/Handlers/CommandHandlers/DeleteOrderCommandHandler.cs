using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;

    public DeleteOrderCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task<CustomResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            order.AddDomainEvent(new OrderDeletedEvent(order));
            
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            await _context.PublishDomainEvents();
            
            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "DeleteTodoItemCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "DeleteTodoItemCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}