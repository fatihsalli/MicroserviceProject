using MediatR;
using MicroserviceProject.Services.User.Domain.Events;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent}", notification.GetType().Name);
        
        Log.Information("MicroserviceProject Domain Event:");

        return Task.CompletedTask;
    }
}