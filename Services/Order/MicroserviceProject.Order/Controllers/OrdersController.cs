using System.Net;
using MicroserviceProject.Order.Service;
using MicroserviceProject.Shared.BaseController;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using OrderModel = MicroserviceProject.Shared.Models.Order;
using Microsoft.Extensions.Options;

namespace MicroserviceProject.Order.Controllers;

public class OrdersController:CustomBaseController
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(CustomResponse<List<OrderModel>>), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        var orders = _orderService.GetAll();
        return CreateActionResult(CustomResponse<List<OrderModel>>.Success(200,orders));
    }
    
    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<OrderModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        var order = _orderService.GetById(id);
        return CreateActionResult(CustomResponse<OrderModel>.Success(200, order));
    }
    
    

    [HttpPost]
    public IActionResult Save(OrderModel order)
    {
        var response = _orderService.Create(order);
        return CreateActionResult((CustomResponse<OrderModel>.Success(201, response)));
    }
    
    
}