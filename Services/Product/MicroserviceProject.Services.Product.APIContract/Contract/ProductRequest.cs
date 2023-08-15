﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Contract
{
    public class ProductRequest
    {
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }


    }
}
