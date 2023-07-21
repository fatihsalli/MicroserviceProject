using System.Net.Http.Json;
using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using MongoDB.Driver;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Queries.GetUserByIdWithOrders;

public record GetUserByIdWithOrdersQuery(string Id) : IRequest<CustomResponse<UserWithOrdersResponse>>
{

}

public class GetUserByIdWithOrdersQueryHandler : IRequestHandler<GetUserByIdWithOrdersQuery,CustomResponse<UserWithOrdersResponse>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public GetUserByIdWithOrdersQueryHandler(IUserDbContext context, IMapper mapper,HttpClient httpClient)
    {
        _context = context;
        _mapper = mapper;
        _httpClient = httpClient;
    }

    public async Task<CustomResponse<UserWithOrdersResponse>> Handle(GetUserByIdWithOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.Find(x => x.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (user==null)
                throw new NotFoundException("user",request.Id);
            
            // Get Orders
            string userMicroserviceBaseUrl = "http://localhost:5011/api/orders";
            string requestUrl = userMicroserviceBaseUrl+$"?userId={request.Id}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl,cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var responseFail = await response.Content.ReadFromJsonAsync<CustomResponse<NoContent>>(cancellationToken: cancellationToken);
                throw new Exception($"Errors:{responseFail.Errors} - StatusCode:{responseFail.StatusCode}");
            }
            
            var responseSuccess = await response.Content.ReadFromJsonAsync<CustomResponse<List<OrderResponse>>>(cancellationToken: cancellationToken);
            
            var userResponse = _mapper.Map<UserResponse>(user);

            var result = new UserWithOrdersResponse {User = userResponse,Orders = responseSuccess.Data};

            return CustomResponse<UserWithOrdersResponse>.Success(200,result);
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