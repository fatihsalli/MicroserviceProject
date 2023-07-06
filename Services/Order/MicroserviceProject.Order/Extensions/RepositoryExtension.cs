using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Repositories;
using MongoDB.Driver;

namespace MicroserviceProject.Order.Extensions;

public static class RepositoryExtension
{
    public static void AddRepositoryExtension(this IServiceCollection services,Config config)
    {
        var mongoClient = new MongoClient(config.Database.Connection);
        var database = mongoClient.GetDatabase(config.Database.DatabaseName);
        var collectionName = config.Database.OrderCollectionName;
            
        services.AddScoped<IGenericRepository<Shared.Models.Order>>(serviceProvider => new GenericRepository<Shared.Models.Order>(database,collectionName));
    }
}