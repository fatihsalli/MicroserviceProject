namespace MicroserviceProject.Shared.Models;

public abstract class BaseModel
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
}