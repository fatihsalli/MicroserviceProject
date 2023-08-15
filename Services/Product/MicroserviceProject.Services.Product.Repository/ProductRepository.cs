using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Repository
{
    public class ProductRepository : GenericRepository<Domain.ProductAggregate.Product>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
