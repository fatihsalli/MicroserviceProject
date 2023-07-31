using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Services.User.Application.Users.Queries.GetAllUsers;
using MicroserviceProject.Shared.Responses;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Application.Users.Handlers.QueryHandlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, CustomResponse<List<UserResponse>>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<List<UserResponse>>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _context.Users.Find(o => true).ToListAsync(cancellationToken);

        return CustomResponse<List<UserResponse>>
            .Success(200, _mapper.Map<List<UserResponse>>(users));
    }
}