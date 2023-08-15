using MediatR;
using MicroserviceProject.Services.Product.APIContract.Request.Command.Category;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Category;
using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using MicroserviceProject.Services.Product.Domain;
using MicroserviceProject.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Product;
using CategoryModel = MicroserviceProject.Services.Product.Domain.ProductAggregate.Category;

namespace MicroserviceProject.Services.Product.Application.Handler.Command.Category
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseBase<CreateCategory>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IDbContextHandler dbContextHandler)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<CreateCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new CategoryModel(request.Category.CategoryName)
            {
                Id = Guid.NewGuid()
            };

            await _categoryRepository.CreateAsync(category);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            var responseCreateProduct = new ResponseBase<CreateCategory>
            {
                Data = new CreateCategory
                {
                    CategoryId = category.Id
                },
                Success = true
            };

            return responseCreateProduct;
        }
    }
}
