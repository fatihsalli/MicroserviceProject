using System.Net;
using MediatR;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;
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
    [ProducesResponseType(typeof(CustomResponse<List<OrderResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var response = await _mediator.Send(new GetAllOrdersQuery { });
        return CreateActionResult(response);
    }

    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<OrderResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrderById([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetOrderByIdQuery(id));
        return CreateActionResult(response);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(CustomResponse<List<OrderResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrdersByUserId([FromQuery] string userId)
    {
        var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId });
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

    [HttpPut("{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateOrderAddress([FromRoute] string id, [FromBody] AddressRequest addressRequest)
    {
        var response = await _mediator.Send(new UpdateOrderCommand { Id = id, Address = addressRequest });
        return CreateActionResult(response);
    }

    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteOrder([FromRoute] string id)
    {
        var response = await _mediator.Send(new DeleteOrderCommand(id));
        return CreateActionResult(response);
    }
}