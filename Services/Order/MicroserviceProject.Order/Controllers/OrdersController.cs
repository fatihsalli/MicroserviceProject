using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.ControllerBases;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroserviceProject.Order.Controllers;

public class OrdersController:CustomBaseController
{
    private readonly Config _config;
    
    public OrdersController(IOptions<Config> config)
    {
        _config = config.Value;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var word = _config.Database.DatabaseName;
        
        return CreateActionResult(CustomResponse<NoContent>.Success(200));
    }
    
    
}