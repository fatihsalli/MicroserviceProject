using MicroserviceProject.Services.User.Domain.Common;

namespace MicroserviceProject.Services.User.Domain.Events;

public class UserUpdatedEvent : BaseEvent
{
    public Entities.User User { get; }

    public UserUpdatedEvent(Entities.User user)
    {
        User = user;
    }
}