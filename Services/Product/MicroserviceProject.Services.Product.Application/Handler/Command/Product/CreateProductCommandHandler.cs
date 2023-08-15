using MediatR;
using MicroserviceProject.Services.Product.APIContract.Request.Command.Product;
using MicroserviceProject.Services.Product.APIContract.Response.Command.Product;
using MicroserviceProject.Services.Product.Domain.ProductAggregate;
using MicroserviceProject.Shared.Models.Responses;
using ProductModel = MicroserviceProject.Services.Product.Domain.ProductAggregate.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceProject.Services.Product.Domain;

namespace MicroserviceProject.Services.Product.Application.Handler.Command.Product
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseBase<CreateProduct>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public CreateProductCommandHandler(IProductRepository productRepository, IDbContextHandler dbContextHandler)
        {
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<CreateProduct>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new ProductModel
                (request.Product.CategoryId, request.Product.ProductName, request.Product.Stock)
            {
                Id = Guid.NewGuid()
            };

            await _productRepository.SaveAsync(product);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            var responseCreateProduct = new ResponseBase<CreateProduct>
            {
                Data = new CreateProduct
                {
                    ProductId = product.Id
                },
                Success = true
                
            };

            return responseCreateProduct;
        }
    }
}
