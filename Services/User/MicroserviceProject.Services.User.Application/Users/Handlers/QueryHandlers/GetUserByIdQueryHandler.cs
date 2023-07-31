using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Services.User.Application.Users.Queries.GetUserById;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using MongoDB.Driver;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Handlers.QueryHandlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery,CustomResponse<UserResponse>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.Find(x => x.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (user==null)
                throw new NotFoundException("user",request.Id);
            
            var userResponse = _mapper.Map<UserResponse>(user);

            return CustomResponse<UserResponse>.Success(200,userResponse);
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Information(ex, "GetUserByIdQueryHandler exception. Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            
            Log.Error(ex, "GetUserByIdQueryHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}