using MicroserviceProject.Services.Product.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository.Mapper
{
    public abstract class BaseEntityMap<TEntity> where TEntity : Entity
    {
        protected abstract void Map(EntityTypeBuilder<TEntity> eb);

        public void BaseMap(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>(bi =>
            {
                bi.Property(b => b.CreatedDate).HasColumnType("datetime");
                bi.Property(b => b.ModifiedDate).HasColumnType("datetime");
                bi.Property(b => b.IsActive).HasColumnType("bit");
                bi.HasKey(b => b.Id);
                Map(bi);
            });
        }

    }
}
