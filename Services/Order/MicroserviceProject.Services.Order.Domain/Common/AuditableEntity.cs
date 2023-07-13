namespace MicroserviceProject.Services.Order.Domain.Common;

public abstract class AuditableEntity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
    
}