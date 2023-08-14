using MicroserviceProject.Services.Product.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.ProductAggregate
{
    public class ProductModel : Entity
    {
        public Guid CategoryId { get; protected set; }
        public string ProductName { get; protected set; }
        public int Stock { get; protected set; }

        private readonly List<CategoryModel> _categories;
        public ICollection<CategoryModel> Categories => _categories;

        protected ProductModel()
        {
            _categories = new List<CategoryModel>();
        }

        // This ile "Product()" çalıştırılıyor.
        public ProductModel(Guid categoryId, string productName, int stock) : this()
        {
            CategoryId = categoryId;
            ProductName = productName;
            Stock = stock;
        }


    }
}
