using System.Net;
using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;
using MicroserviceProject.Shared.BaseController;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceProject.Services.Order.API.Controllers;

public class OrdersController : CustomBaseController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var response = await _mediator.Send(new GetAllOrdersQuery { });
        return CreateActionResult(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CustomResponse<CreatedOrderResponse>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveOrder([FromBody] CreateOrderCommand createOrderCommand)
    {
        var response = await _mediator.Send(createOrderCommand);
        return CreateActionResult(response);
    }
    
    
}