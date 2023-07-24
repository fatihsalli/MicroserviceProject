using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Domain.Events;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using MongoDB.Driver;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<CustomResponse<bool>>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,CustomResponse<bool>>
{
    private readonly IUserDbContext _context;

    public DeleteUserCommandHandler(IUserDbContext context)
    {
        _context = context;
    }

    public async Task<CustomResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // User Check
            var user = await _context.Users.Find(x => x.Id == request.Id).SingleOrDefaultAsync(cancellationToken);
            if (user==null)
                throw new NotFoundException("user",request.Id);
            
            // User Delete
            var result = await _context.Users.DeleteOneAsync(x=>x.Id==request.Id,cancellationToken);
            if (result.DeletedCount < 1)
                throw new Exception($"User with id {request.Id} cannot delete!");

            // Add Event
            user.AddDomainEvent(new UserDeletedEvent(user));

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