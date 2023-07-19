using MediatR;
using MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;
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
    
    [HttpPost]
    public async Task<IActionResult> SaveOrder([FromBody] CreateUserCommand createUserCommand)
    {
        var response = await _mediator.Send(createUserCommand);
        return CreateActionResult(response);
    }
}