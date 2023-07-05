using System.Linq.Expressions;

namespace MicroserviceProject.Shared.Repositories;

public interface IGenericRepository<TEntity>
{
    public IEnumerable<TEntity> GetAll();
    public TEntity GetById(string id);
    public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filterExpression);
    public void Insert(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(string id);
}