using MediatR;
using MicroserviceProject.Services.Product.APIContract.Contract;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Product;
using MicroserviceProject.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Request.Command.Product
{
    public class CreateProductCommand : IRequest<ResponseBase<CreateProduct>>
    {
        public ProductRequest Product { get; set; }
    }
}
