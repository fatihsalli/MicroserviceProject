using MediatR;
using MicroserviceProject.Services.Product.APIContract.Request.Command.Product;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceProject.Services.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveProduct(CreateProductCommand request)
        {
            var createSellerResult = await _mediator.Send(request);
            return Ok(createSellerResult);
        }



    }
}
