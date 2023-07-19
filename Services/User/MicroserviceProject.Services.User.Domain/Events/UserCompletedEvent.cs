using MicroserviceProject.Services.User.Domain.Common;

namespace MicroserviceProject.Services.User.Domain.Events;

public class UserCompletedEvent : BaseEvent
{
    public Entities.User User { get; }

    public UserCompletedEvent(Entities.User user)
    {
        User = user;
    }
}