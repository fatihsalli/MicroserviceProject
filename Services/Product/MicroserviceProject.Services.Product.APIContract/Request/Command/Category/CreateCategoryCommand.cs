using Amazon.Runtime.Internal;
using MediatR;
using MicroserviceProject.Services.Product.APIContract.Contract;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Category;
using MicroserviceProject.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Request.Command.Category
{
    public class CreateCategoryCommand : IRequest<ResponseBase<CreateCategory>>
    {
        public CategoryRequest Category { get; set; }

    }
}
