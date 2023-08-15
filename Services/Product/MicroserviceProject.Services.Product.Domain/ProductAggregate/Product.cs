using MicroserviceProject.Services.Product.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.ProductAggregate
{
    public class Product : Entity
    {
        public Guid CategoryId { get; protected set; }
        public string ProductName { get; protected set; }
        public int Stock { get; protected set; }
        public Category Category { get; protected set; }

        // This ile "Product()" çalıştırılıyor.
        public Product(Guid categoryId, string productName, int stock)
        {
            CategoryId = categoryId;
            ProductName = productName;
            Stock = stock;
        }


    }
}
