using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Shared.Exceptions;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderDbContext _context;

    public DeleteTodoItemCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);

        _context.Orders.Remove(entity);

        entity.AddDomainEvent(new OrderDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }

}