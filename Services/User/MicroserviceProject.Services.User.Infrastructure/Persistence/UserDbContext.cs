using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Infrastructure.Common;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Infrastructure.Persistence;

public class UserDbContext : IUserDbContext
{
    private readonly IMediator _mediator;
    public UserDbContext(IUserDatabaseSettings userDatabaseSettings,IMediator mediator)
    {
        _mediator = mediator;
        
        var client = new MongoClient(userDatabaseSettings.ConnectionString);
        var database = client.GetDatabase(userDatabaseSettings.DatabaseName);
        Users = database.GetCollection<Domain.Entities.User>(userDatabaseSettings.UserCollectionName);
    }

    public IMongoCollection<Domain.Entities.User> Users { get; }

    public async Task PublishDomainEvents(Domain.Entities.User user)
    {
        await _mediator.DispatchDomainEvents(user);
    } 
}