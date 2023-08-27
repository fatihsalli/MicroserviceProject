﻿namespace MicroserviceProject.Services.Order.Application.Helpers;

public class CorrelationIdGenerator : ICorrelationIdGenerator
{
    private string _correlationId = Guid.NewGuid().ToString();

    public string Get() => _correlationId;

    public void Set(string correlationId) { 
        _correlationId = correlationId;
    }
}