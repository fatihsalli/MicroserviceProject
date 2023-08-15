using MicroserviceProject.Services.Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly ProductDbContext _dbContext;

        public DbContextHandler(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await SaveChangesAsync(CancellationToken.None);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw ex;
            }
        }
    }
}
