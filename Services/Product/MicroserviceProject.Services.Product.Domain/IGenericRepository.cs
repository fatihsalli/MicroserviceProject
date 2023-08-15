using MicroserviceProject.Services.Product.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetByIdAsync(Guid id, bool isActive = true);
        Task<List<TEntity>> AllAsync(bool isActive = true);
        Task CreateAsync(TEntity entity);
    }
}
