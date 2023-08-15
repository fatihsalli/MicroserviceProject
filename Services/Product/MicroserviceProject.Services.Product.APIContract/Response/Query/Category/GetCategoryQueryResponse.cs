using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Response.Query.Category
{
    public class GetCategoryQueryResponse
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }

    }
}
