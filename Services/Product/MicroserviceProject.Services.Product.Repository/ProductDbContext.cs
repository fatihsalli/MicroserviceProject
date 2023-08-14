using MicroserviceProject.Services.Product.Domain.Entitites;
using MicroserviceProject.Services.Product.Repository.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository
{
    public class ProductDbContext : DbContext
    {
        private readonly ISaveChangesInterceptor _makerCheckerEntitySaveInterceptor;

        public ProductDbContext(DbContextOptions options, ISaveChangesInterceptor makerCheckerEntitySaveInterceptor = null) : base(options)
        {
            _makerCheckerEntitySaveInterceptor = makerCheckerEntitySaveInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
            optionsBuilder.AddInterceptors(_makerCheckerEntitySaveInterceptor);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ProductMapper().BaseMap(modelBuilder);
            new CategoryMapper().BaseMap(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var added = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in added)
            {
                if (entity is Entity track)
                {
                    track.CreatedDate = DateTime.Now;
                    track.IsActive = true;
                }
            }

            var modified = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in modified)
            {
                if (entity is Entity track)
                {
                    track.ModifiedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
