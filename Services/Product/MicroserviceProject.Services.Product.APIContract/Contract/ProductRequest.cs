using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Contract
{
    public class ProductRequest
    {
        public Guid CategoryId { get; protected set; }
        public string ProductName { get; protected set; }
        public int Stock { get; protected set; }


    }
}
