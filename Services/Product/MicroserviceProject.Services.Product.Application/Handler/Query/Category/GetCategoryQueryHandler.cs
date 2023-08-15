using MediatR;
using MicroserviceProject.Services.Product.APIContract.Request.Query.Category;
using MicroserviceProject.Services.Product.APIContract.Response.Query.Category;
using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using MicroserviceProject.Services.Product.Domain;
using MicroserviceProject.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Category;

namespace MicroserviceProject.Services.Product.Application.Handler.Query.Category
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, ResponseBase<GetCategoryQueryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseBase<GetCategoryQueryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category=await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
                throw new Exception($"Category with id ({request.CategoryId}) not found.");

            var responseCreateProduct = new ResponseBase<GetCategoryQueryResponse>
            {
                Data = new GetCategoryQueryResponse
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                },
                Success = true
            };

            return responseCreateProduct;
        }
    }
}
