using System.Linq.Expressions;
using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(string id);
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    IQueryable<T> GetAll();
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<List<T>> GetAllWithIncludeAsync(params string[] includes);
    Task<T> GetByIdWithIncludeAsync(string id, params string[] includes);
    IEnumerable<T> WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes);
}