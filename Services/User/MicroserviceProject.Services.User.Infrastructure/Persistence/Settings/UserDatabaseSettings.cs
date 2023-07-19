using MicroserviceProject.Services.User.Application.Common.Interfaces;

namespace MicroserviceProject.Services.User.Infrastructure.Persistence.Settings;

public class UserDatabaseSettings:IUserDatabaseSettings
{
    public string UserCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}