using System.Linq.Expressions;
using MicroserviceProject.Shared.Models;

namespace MicroserviceProject.Shared.BaseRepository;

public interface IGenericRepository<TEntity> where TEntity : BaseModel
{
    IEnumerable<TEntity> GetAll();
    TEntity GetById(string id);
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filterExpression);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(string id);
}