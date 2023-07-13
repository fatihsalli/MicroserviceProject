using MediatR;
using MicroserviceProject.Services.Order.Application.Commands;
using MicroserviceProject.Services.Order.Application.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Application.Queries;
using MicroserviceProject.Shared.BaseController;
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
    public async Task<IActionResult> GetOrders([FromQuery] string userId)
    {
        var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId });
        
        return CreateActionResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> SaveOrder([FromBody] CreateOrderCommand createOrderCommand)
    {
        var response = await _mediator.Send(createOrderCommand);

        return CreateActionResult(response);
    }
    
    
}