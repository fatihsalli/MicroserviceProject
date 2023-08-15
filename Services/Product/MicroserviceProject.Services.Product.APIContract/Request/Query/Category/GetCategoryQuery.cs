using Amazon.Runtime.Internal;
using MediatR;
using MicroserviceProject.Services.Product.APIContract.Response.Query.Category;
using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using MicroserviceProject.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.APIContract.Request.Query.Category
{
    public class GetCategoryQuery : IRequest<ResponseBase<GetCategoryQueryResponse>>
    {
        public Guid CategoryId { get; set; }
    }
}
