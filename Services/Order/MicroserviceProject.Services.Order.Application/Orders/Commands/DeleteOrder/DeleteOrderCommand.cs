﻿using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(string Id) : IRequest<CustomResponse<bool>>;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteOrderCommand,CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;

    public DeleteTodoItemCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task<CustomResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x=>x.Id==request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);

            _context.Orders.Remove(order);

            order.AddDomainEvent(new OrderDeletedEvent(order));

            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Information(ex, "DeleteTodoItemCommandHandler exception. Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            
            Log.Error(ex, "DeleteTodoItemCommandHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}