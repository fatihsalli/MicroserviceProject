using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Shared.Models.Responses
{
    public class OrderCustomResponse
    {
        public string Id { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
