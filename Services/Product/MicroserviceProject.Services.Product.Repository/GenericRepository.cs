using MicroserviceProject.Services.Product.Domain;
using MicroserviceProject.Services.Product.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _dbContext;
        public readonly DbSet<TEntity> _entities;

        public GenericRepository(DbContext dbContext)
        {
            _entities = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync(Guid id,bool isActive = true)
        {
            return await _entities
                .SingleOrDefaultAsync(s => s.Id == id && s.IsActive == isActive);
        }

        public async Task<List<TEntity>> AllAsync(bool isActive = true)
        {
            return await _entities
                .Where(s => s.IsActive==isActive)
                .AsQueryable()
                .ToListAsync();
        }


    }
}
