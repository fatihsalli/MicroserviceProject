using System.Text;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Domain.ValueObjects;
using MicroserviceProject.Shared.Responses;

namespace MicroserviceProject.Services.User.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CustomResponse<Domain.Entities.User>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<Address> Addresses { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CustomResponse<Domain.Entities.User>>
{
    private readonly IUserDbContext _context;

    public CreateUserCommandHandler(IUserDbContext context)
    {
        _context = context;
    }

    public async Task<CustomResponse<Domain.Entities.User>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var newUser = new Domain.Entities.User();

        newUser.Id = Guid.NewGuid().ToString();
        newUser.Username = request.Username;
        newUser.Email = request.Email;
        newUser.Password = Encoding.UTF8.GetBytes(request.Password);
        newUser.FullName = request.FullName;
        newUser.Addresses = request.Addresses;
        newUser.CreatedAt = DateTime.Now;
        newUser.UpdadetAt = newUser.CreatedAt;

        await _context.Users.InsertOneAsync(newUser, cancellationToken);
        return CustomResponse<Domain.Entities.User>.Success(201, newUser);
    }
}