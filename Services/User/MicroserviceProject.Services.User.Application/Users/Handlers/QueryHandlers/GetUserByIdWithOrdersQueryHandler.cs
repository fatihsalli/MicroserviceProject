using System.Net.Http.Json;
using AutoMapper;
using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Application.Users.Queries.GetUserByIdWithOrders;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Responses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace MicroserviceProject.Services.User.Application.Users.Handlers.QueryHandlers;


public class GetUserByIdWithOrdersQueryHandler : IRequestHandler<GetUserByIdWithOrdersQuery,CustomResponse<UserWithOrdersResponse>>
{
    private readonly IUserDbContext _context;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly Config _config;

    public GetUserByIdWithOrdersQueryHandler(IUserDbContext context, IMapper mapper,HttpClient httpClient,IOptions<Config> config)
    {
        _context = context;
        _mapper = mapper;
        _httpClient = httpClient;
        _config = config.Value;
    }

    public async Task<CustomResponse<UserWithOrdersResponse>> Handle(GetUserByIdWithOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.Find(x => x.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (user==null)
                throw new NotFoundException("user",request.Id);
            
            // Get Orders with HttpClient
            string requestUrl = _config.HttpClient.OrderApi+$"/GetOrdersByUserId?userId={request.Id}";
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