using MicroserviceProject.Shared.Models;

namespace MicroserviceProject.Shared.BaseService;

public interface IGenericService<TEntity> where TEntity : BaseModel
{
    List<TEntity> GetAll();
    TEntity Create(TEntity entity);
    TEntity GetById(string id);
}