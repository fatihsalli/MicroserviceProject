using MediatR;
using MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Repository;
using MicroserviceProject.Services.Order.Repository.Interfaces;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, CustomResponse<bool>>
{
    private readonly OrderDbContext _context;

    public DeleteOrderCommandHandler(OrderDbContext context)
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
            
            // SaveChangesAsync metodundan önce çalışmalı diğer durumda "DispatchDomainEvents" metodunda context üzerinden "DomainEvent" nesnelerini çekemiyoruz.
            await _context.PublishDomainEvents();
            await _context.SaveChangesAsync(cancellationToken);
            
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