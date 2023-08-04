using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;
using MicroserviceProject.Services.User.Domain.Events;
using MicroserviceProject.Services.User.Domain.ValueObjects;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Application.Users.Handlers.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CustomResponse<UserCreatedResponse>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateUserCommandHandler(IUserDbContext context,IMapper mapper,IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CustomResponse<UserCreatedResponse>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var newUser = new Domain.Entities.User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            Email = request.Email,
            Password = PasswordCheck.HashPassword(request.Password),
            FullName = request.FullName,
            Addresses = _mapper.Map<List<Address>>(request.Addresses),
            CreatedAt = DateTime.Now
        };

        newUser.UpdadetAt = newUser.CreatedAt;
        
        newUser.AddDomainEvent(new UserCreatedEvent(newUser));
        await _context.PublishDomainEvents(newUser);

        await _context.Users.InsertOneAsync(newUser, new InsertOneOptions(), cancellationToken);
        
        return CustomResponse<UserCreatedResponse>
            .Success(201, new UserCreatedResponse{UserId = newUser.Id});
    }
}