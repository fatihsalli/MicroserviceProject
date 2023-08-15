using MicroserviceProject.Services.Product.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.ProductAggregate
{
    public class Category : Entity
    {
        public string CategoryName { get;protected set; }

        private readonly List<Product> _products;
        public ICollection<Product> Products => _products;

        protected Category()
        {
            _products = new List<Product>();
        }

        public Category(string categoryName) : this()
        {
            CategoryName = categoryName;
        }

    }
}
