using System.Text.Json;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace MicroserviceProject.Shared.Extensions;

public static class UseCustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionFeature.Error is ValidationException validationException)
                {
                    context.Response.StatusCode = 400;
                    
                    var failures = validationException.Errors;
                    
                    
                    var response = CustomResponse<NoContent>.Fail(400, failures);
                    
                    await context.Response.WriteAsync(JsonSerializer.Serialize(failures));
                    
                    
                }
                else
                {
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        ValidationException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;
                    var response = CustomResponse<NoContent>.Fail(statusCode, exceptionFeature.Error.Message);
                    //Custom middleware oluşturduğumuz için kendimiz json formatına serialize etmemiz gerekir.
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}