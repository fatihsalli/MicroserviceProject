using MediatR;
using MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;
using MicroserviceProject.Services.User.Application.Users.Queries.GetAllUsers;
using MicroserviceProject.Shared.BaseController;
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
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _mediator.Send(new GetAllUsersQuery());
        return CreateActionResult(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveUser([FromBody] CreateUserCommand createUserCommand)
    {
        var response = await _mediator.Send(createUserCommand);
        return CreateActionResult(response);
    }
}