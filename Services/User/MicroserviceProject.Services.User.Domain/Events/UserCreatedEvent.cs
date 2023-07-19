using MicroserviceProject.Services.User.Domain.Common;

namespace MicroserviceProject.Services.User.Domain.Events;

public class UserCreatedEvent : BaseEvent
{
    public Entities.User User { get; }

    public UserCreatedEvent(Entities.User user)
    {
        User = user;
    }
}