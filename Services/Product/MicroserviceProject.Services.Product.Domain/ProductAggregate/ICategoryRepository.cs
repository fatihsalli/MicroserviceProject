using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.ProductAggregate
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }
}
