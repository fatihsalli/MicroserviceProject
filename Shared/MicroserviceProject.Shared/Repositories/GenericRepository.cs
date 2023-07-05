using System.Linq.Expressions;
using MicroserviceProject.Shared.Models;
using MongoDB.Driver;

namespace MicroserviceProject.Shared.Repositories;

public class GenericRepository<Entity>:IGenericRepository<Entity> where Entity:BaseModel
{
    private readonly IMongoCollection<Entity> _collection;
    
    public GenericRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<Entity>(collectionName);
    }
    
    public IEnumerable<Entity> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }
    
    public Entity GetById(string id)
    {
        var filter = Builders<Entity>.Filter.Eq("_id", id);
        return _collection.Find(filter).FirstOrDefault();
    }
    
    public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToList();
    }
    
    public void Insert(Entity entity)
    {
        _collection.InsertOne(entity);
    }
    
    public void Update(Entity entity)
    {
        var id = entity.GetType().GetProperty("ID").GetValue(entity).ToString();
        var filter = Builders<Entity>.Filter.Eq("_id", id);
        _collection.ReplaceOne(filter, entity);
    }
    
    public void Delete(string id)
    {
        var filter = Builders<Entity>.Filter.Eq("_id", id);
        _collection.DeleteOne(filter);
    }
}