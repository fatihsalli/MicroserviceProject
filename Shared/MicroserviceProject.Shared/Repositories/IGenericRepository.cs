using System.Linq.Expressions;

namespace MicroserviceProject.Shared.Repositories;

public interface IGenericRepository<Entity>
{
    public IEnumerable<Entity> GetAll();
    public Entity GetById(string id);
    public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> filterExpression);
    public void Insert(Entity entity);
    public void Update(Entity entity);
    public void Delete(string id);
}