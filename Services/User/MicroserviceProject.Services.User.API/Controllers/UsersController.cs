using System.Net;
using MediatR;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;
using MicroserviceProject.Services.User.Application.Users.Commands.UpdateUser;
using MicroserviceProject.Services.User.Application.Users.Queries.GetAllUsers;
using MicroserviceProject.Services.User.Application.Users.Queries.GetUserById;
using MicroserviceProject.Services.User.Application.Users.Queries.GetUserByIdWithOrders;
using MicroserviceProject.Shared.BaseController;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceProject.Services.User.API.Controllers;

public class UsersController : CustomBaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(CustomResponse<List<UserResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _mediator.Send(new GetAllUsersQuery());
        return CreateActionResult(response);
    }
    
    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<UserResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(id));
        return CreateActionResult(response);
    }
    
    [HttpGet("[action]/{id:length(36)}")]
    [ProducesResponseType(typeof(CustomResponse<UserResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetUserByIdWithOrders([FromRoute] string id)
    {
        var response = await _mediator.Send(new GetUserByIdWithOrdersQuery(id));
        return CreateActionResult(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CustomResponse<CreatedUserResponse>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveUser([FromBody] CreateUserCommand createUserCommand)
    {
        var response = await _mediator.Send(createUserCommand);
        return CreateActionResult(response);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(CustomResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CustomResponse<NoContent>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveUser([FromBody] UpdateUserCommand updateUserCommand)
    {
        var response = await _mediator.Send(updateUserCommand);
        return CreateActionResult(response);
    }
}