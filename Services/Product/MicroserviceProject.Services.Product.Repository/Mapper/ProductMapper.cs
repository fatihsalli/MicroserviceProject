using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository.Mapper
{
    public class ProductMapper : BaseEntityMap<Domain.ProductAggregate.Product>
    {
        protected override void Map(EntityTypeBuilder<Domain.ProductAggregate.Product> eb)
        {
            eb.Property(p => p.CategoryId).HasColumnType("uniqueidentifier");
            eb.Property(p => p.ProductName).HasColumnType("nvarchar(50)");
            eb.Property(p => p.Stock).HasColumnType("int");

            eb.ToTable("Product");
        }
    }
}
