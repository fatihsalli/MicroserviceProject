namespace MicroserviceProject.Shared.Models;

public class User : BaseModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public List<Address> Addresses { get; set; }
}