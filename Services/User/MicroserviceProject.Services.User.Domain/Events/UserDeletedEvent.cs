using MicroserviceProject.Services.User.Domain.Common;

namespace MicroserviceProject.Services.User.Domain.Events;

public class UserDeletedEvent : BaseEvent
{
    public Entities.User User { get; }

    public UserDeletedEvent(Entities.User user)
    {
        User = user;
    }
}