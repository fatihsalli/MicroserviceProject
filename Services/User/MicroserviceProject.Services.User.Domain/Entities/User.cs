using MicroserviceProject.Services.User.Domain.Common;
using MicroserviceProject.Services.User.Domain.ValueObjects;

namespace MicroserviceProject.Services.User.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public string FullName { get; set; }
    public List<Address> Addresses { get; set; }
}