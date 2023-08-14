using MicroserviceProject.Services.Product.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.ProductAggregate
{
    public class CategoryModel : Entity
    {
        public string CategoryName { get;protected set; }
    }
}
