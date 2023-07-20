using System.Text;
using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Dtos.Requests;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Services.User.Domain.Events;
using MicroserviceProject.Services.User.Domain.ValueObjects;
using MicroserviceProject.Shared.Responses;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CustomResponse<CreatedUserResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<AddressRequest> Addresses { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CustomResponse<CreatedUserResponse>>
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

    public async Task<CustomResponse<CreatedUserResponse>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var newUser = new Domain.Entities.User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            Email = request.Email,
            Password = Encoding.UTF8.GetBytes(request.Password),
            FullName = request.FullName,
            Addresses = _mapper.Map<List<Address>>(request.Addresses),
            CreatedAt = DateTime.Now
        };

        newUser.UpdadetAt = newUser.CreatedAt;
        
        newUser.AddDomainEvent(new UserCreatedEvent(newUser));
        await _context.PublishDomainEvents(newUser);

        await _context.Users.InsertOneAsync(newUser, new InsertOneOptions(), cancellationToken);
        
        return CustomResponse<CreatedUserResponse>
            .Success(201, new CreatedUserResponse{UserId = newUser.Id});
    }
}