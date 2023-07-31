using MediatR;
using MicroserviceProject.Services.User.Domain.Events;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Handlers.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        Log.Information("MicroserviceProject Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}