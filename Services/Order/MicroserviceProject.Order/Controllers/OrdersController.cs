using MicroserviceProject.Order.Service;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.ControllerBases;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroserviceProject.Order.Controllers;

public class OrdersController:CustomBaseController
{
    private readonly IOrderService _service;
    
    public OrdersController(IOrderService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _service.GetAll();
        return CreateActionResult(CustomResponse<List<Shared.Models.Order>>.Success(200,orders));
    }

    [HttpPost]
    public IActionResult Save(Shared.Models.Order order)
    {
        var response = _service.Create(order);
        return CreateActionResult((CustomResponse<Shared.Models.Order>.Success(201, response)));
    }
    
    
}