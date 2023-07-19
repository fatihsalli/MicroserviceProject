using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Infrastructure.Persistence;

public class UserDbContext : IUserDbContext
{
    public UserDbContext(IUserDatabaseSettings userDatabaseSettings)
    {
        var client = new MongoClient(userDatabaseSettings.ConnectionString);

        var database = client.GetDatabase(userDatabaseSettings.DatabaseName);

        Users = database.GetCollection<Domain.Entities.User>(userDatabaseSettings.UserCollectionName);
    }

    public IMongoCollection<Domain.Entities.User> Users { get; }
}