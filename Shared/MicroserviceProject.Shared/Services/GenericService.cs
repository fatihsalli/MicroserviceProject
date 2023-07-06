using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Repositories;

namespace MicroserviceProject.Shared.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : BaseModel
{
    private readonly IGenericRepository<TEntity> _repository;
    public GenericService(IGenericRepository<TEntity> repository)
    {
        _repository = repository;
    }
    
    public virtual List<TEntity> GetAll()
    {
        try
        {
            var entities = _repository.GetAll();
            return entities.ToList();
        }
        catch (Exception ex)
        {
            // TODO: Log and error handler
            Console.WriteLine(ex);
            throw;
        }
    }

    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedAt=DateTime.Now;
            entity.UpdadetAt = entity.CreatedAt;
            _repository.Insert(entity);
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    
}