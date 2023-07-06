using MicroserviceProject.Shared.BaseRepository;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;

namespace MicroserviceProject.Shared.BaseService;

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

    public virtual TEntity GetById(string id)
    {
        try
        {
            var entity = _repository.GetById(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with {id} cannot found!");

            return entity;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
                throw new NotFoundException($"Not Found Exception!:{ex.Message}");

            throw new Exception($"Something went wrong!:{ex.Message}");
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
            throw new Exception($"Something went wrong!:{ex.Message}");
        }
    }
    
    
}