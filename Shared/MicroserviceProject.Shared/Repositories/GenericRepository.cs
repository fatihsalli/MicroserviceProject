using System.Linq.Expressions;
using MicroserviceProject.Shared.Models;
using MongoDB.Driver;

namespace MicroserviceProject.Shared.Repositories;

public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity:BaseModel
{
    private readonly IMongoCollection<TEntity> _collection;
    public GenericRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }
    
    public IEnumerable<TEntity> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }
    
    public TEntity GetById(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return _collection.Find(filter).FirstOrDefault();
    }
    
    public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToList();
    }
    
    public void Insert(TEntity entity)
    {
        _collection.InsertOne(entity);
    }
    
    public void Update(TEntity entity)
    {
        var id = entity.GetType().GetProperty("ID").GetValue(entity).ToString();
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        _collection.ReplaceOne(filter, entity);
    }
    
    public void Delete(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        _collection.DeleteOne(filter);
    }
}