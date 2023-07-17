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

                switch (exceptionFeature.Error)
                {
                    case ValidationException validationException:
                    {
                        context.Response.StatusCode = 400;
                        var failures = validationException.Errors;
                        var response = CustomResponse<NoContent>.Fail(400, failures);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }
                    case ClientSideException:
                    {
                        context.Response.StatusCode = 400;
                        var response = CustomResponse<NoContent>.Fail(400, exceptionFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }
                    case NotFoundException:
                    {
                        context.Response.StatusCode = 404;
                        var response = CustomResponse<NoContent>.Fail(400, exceptionFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }
                    default:
                    {
                        context.Response.StatusCode = 500;
                        var responseServer = CustomResponse<NoContent>.Fail(500, exceptionFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(responseServer));
                        return;
                    }
                }
            });
        });
    }
}