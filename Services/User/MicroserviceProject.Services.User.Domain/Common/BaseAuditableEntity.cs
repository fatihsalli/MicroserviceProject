namespace MicroserviceProject.Services.User.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
}