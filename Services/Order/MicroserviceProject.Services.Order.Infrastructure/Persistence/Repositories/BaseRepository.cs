using System.Linq.Expressions;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;
using MicroserviceProject.Services.Order.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly OrderDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(OrderDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }
    
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }
    
    public IQueryable<T> GetAll()
    {
        var entities = _dbSet.AsNoTracking().AsQueryable();
        return entities;
    }
    
    public async Task<List<T>> GetAllWithIncludeAsync(params string[] includes)
    {
        var query = _dbSet.AsQueryable();
        query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        return await query.ToListAsync();
    }
    
    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    public async Task<T> GetByIdWithIncludeAsync(string id, params string[] includes)
    {
        var query = _dbSet.AsQueryable();
        query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
    
    public IEnumerable<T> WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes)
    {
        var query = _dbSet.AsQueryable();
        query = query.Where(expression);
        query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        return query.ToList();
    }
}