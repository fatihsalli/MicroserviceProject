using MicroserviceProject.Shared.Models;

namespace MicroserviceProject.Shared.Services;

public interface IGenericService<TEntity> where TEntity : BaseModel
{
    List<TEntity> GetAll();
    TEntity Create(TEntity entity);
}