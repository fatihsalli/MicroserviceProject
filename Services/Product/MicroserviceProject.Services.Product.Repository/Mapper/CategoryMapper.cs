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
    public class CategoryMapper : BaseEntityMap<Category>
    {
        protected override void Map(EntityTypeBuilder<Category> eb)
        {
            eb.Property(c => c.CategoryName).HasColumnType("nvarchar(50)");

            eb.ToTable("Category");
        }
    }
}
