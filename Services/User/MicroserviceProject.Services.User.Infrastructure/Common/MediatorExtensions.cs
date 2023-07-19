using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Infrastructure.Common;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, IUserDbContext context) 
    {
        var collections = new IMongoCollection<Domain.Entities.User>[]
        {
            context.Users, // IMongoCollection<T> olarak tanımladığımız koleksiyonları ekleyin
        };
        
        var entities = new List<Domain.Entities.User>();
        var domainEvents = new List<BaseEvent>();
        
        foreach (var collection in collections)
        {
            var filter = Builders<Domain.Entities.User>.Filter.Exists("DomainEvents");
            var projection = Builders<Domain.Entities.User>.Projection.Include("DomainEvents");

            var cursor = await collection.FindAsync(filter, new FindOptions<Domain.Entities.User, BsonDocument> { Projection = projection });
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    var entity = BsonSerializer.Deserialize<Domain.Entities.User>(document);
                    entities.Add(entity);
                    domainEvents.AddRange(entity.DomainEvents);
                    entity.ClearDomainEvents();
                }
            }
        }
        
        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}



