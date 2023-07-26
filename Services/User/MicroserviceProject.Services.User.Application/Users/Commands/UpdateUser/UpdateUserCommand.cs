using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Dtos.Requests;
using MicroserviceProject.Services.User.Domain.Events;
using MicroserviceProject.Services.User.Domain.ValueObjects;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<CustomResponse<bool>>
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<AddressRequest> Addresses { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CustomResponse<bool>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<CustomResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
                .Find(x=>x.Id==request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException("user",request.Id);

            var passwordCheck = PasswordCheck.VerifyPassword(request.Password, user.Password);

            if (!passwordCheck)
                throw new ClientSideException("Password is invalid!");

            // MongoDB'de kullanıcıyı güncellemek için önce güncellenecek alanları ayarlıyoruz.
            var update = Builders<Domain.Entities.User>.Update
                .Set(u => u.Username, request.Username)
                .Set(u => u.Email, request.Email)
                .Set(u => u.FullName, request.FullName)
                .Set(u => u.Addresses, _mapper.Map<List<Address>>(request.Addresses))
                .Set(u => u.UpdadetAt, DateTime.Now);
            
            // MongoDB'de güncellemeyi gerçekleştiriyoruz.
            var result = await _context.Users.UpdateOneAsync(
                // Filtre ile güncellenecek kullanıcıyı belirtiyoruz. Burada "_id" alanını kullanıyoruz.
                Builders<Domain.Entities.User>.Filter.Eq(u => u.Id, request.Id),
                update,
                cancellationToken: cancellationToken
            );

            if (result.ModifiedCount < 1)
                throw new Exception("User cannot update!");
            
            // Publish etmek için güncel user modeline ihtiyacım var.
            var newUser = await _context.Users
                .Find(x=>x.Id==request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (newUser == null)
                throw new NotFoundException("user",request.Id);
            
            newUser.AddDomainEvent(new UserUpdatedEvent(newUser));
            await _context.PublishDomainEvents(newUser);

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "UpdateOrderCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                case ClientSideException:
                    Log.Information(ex, "UpdateOrderCommandHandler exception. Client Side Error");
                    throw new ClientSideException($"Client Side Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "UpdateOrderCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}