﻿using MicroserviceProject.Services.Order.Application.Helpers;
using Microsoft.Extensions.Primitives;

namespace MicroserviceProject.Services.Order.API.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationIdHeader = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next) { 
        _next = next; 
    }

    public async Task Invoke(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
    {
        var correlationId = GetCorrelationId(context, correlationIdGenerator);
        AddCorrelationIdHeaderToResponse(context, correlationId);
        await _next(context);
    }

    private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
    {
        if(context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
        {
            correlationIdGenerator.Set(correlationId);
            return correlationId;
        }
        else
        {
            return correlationIdGenerator.Get();
        }
    }

    // TODO: CorrelationId ile çalışmadı
    
    private static void AddCorrelationIdHeaderToResponse(HttpContext context, StringValues correlationId)
    { 
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(_correlationIdHeader, new[] {correlationId.ToString()});
            return Task.CompletedTask;
        });
    }
}