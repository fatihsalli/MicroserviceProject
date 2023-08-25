using System.Net;
using AutoMapper;
using MediatR;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.DeleteOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrderStatus;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetAllOrders;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderById;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByStatus;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersByUserId;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrdersWithPagination;
using MicroserviceProject.Services.Order.Application.Orders.Queries.GetOrderTest;
using MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories.Autofac;
using MicroserviceProject.Shared.BaseController;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Requests;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MicroserviceProject.Services.Order.API.Controllers;

public class OrdersController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAutofacRepoTest _autofacRepoTest;
    private readonly IAutofacRepoTest2 _autofacRepoTest2;

    public OrdersController(IMapper mapper, IMediator mediator, IAutofacRepoTest autofacRepoTest, IAutofacRepoTest2 autofacRepoTest2)
    {
        _mapper = mapper;
        _mediator = mediator;
        _autofacRepoTest = autofacRepoTest;
        _autofacRepoTest2 = autofacRepoTest2;
    }

    [HttpGet]
    [ProducesResponseType(typeof(CustomResponse<List<OrderResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var response = await _mediator.Send(new GetAllOrdersQuery());
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
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrdersByStatus([FromQuery] int statusId)
    {
        var response = await _mediator.Send(new GetOrdersByStatusQuery { StatusId = statusId });
        return CreateActionResult(response);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(CustomResponse<List<OrderResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetOrdersByUserId([FromQuery] string userId)
    {
        var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId });
        return CreateActionResult(response);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetTodoItemsWithPagination([FromQuery] GetOrdersWithPaginationQuery query)
    {
        var response = await _mediator.Send(query);
        return CreateActionResult(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomResponse<OrderCreatedResponse>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveOrder([FromBody] CreateOrderRequest createOrderRequest)
    {
        var createOrderCommand = _mapper.Map<CreateOrderCommand>(createOrderRequest);
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

    [HttpPut("[action]/{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] string id, [FromQuery] int statusId)
    {
        var response = await _mediator.Send(new UpdateOrderStatusCommand { Id = id, StatusId = statusId });
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

    [HttpGet("[action]")]
    public IActionResult AutofacTest()
    {
        _autofacRepoTest.GetRequest();
        _autofacRepoTest2.GetRequest();

        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> TestOrder([FromQuery] string id)
    {
        var response = await _mediator.Send(new GetOrderTestQuery { Id = id });
        return CreateActionResult(response);
    }

}