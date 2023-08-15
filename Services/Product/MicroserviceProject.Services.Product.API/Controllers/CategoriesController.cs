using Azure.Core;
using MediatR;
using MicroserviceProject.Services.Product.APIContract.Request.Command.Category;
using MicroserviceProject.Services.Product.APIContract.Request.Command.Product;
using MicroserviceProject.Services.Product.APIContract.Request.Query.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceProject.Services.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategory([FromQuery] GetCategoryQuery request)
        {
            var getCategoryResult = await _mediator.Send(request);
            return Ok(getCategoryResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveCategory(CreateCategoryCommand request)
        {
            var createCategoryResult = await _mediator.Send(request);
            return Ok(createCategoryResult);
        }
    }
}
