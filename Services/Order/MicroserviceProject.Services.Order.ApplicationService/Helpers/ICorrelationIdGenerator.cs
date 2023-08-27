namespace MicroserviceProject.Services.Order.Application.Helpers;

public interface ICorrelationIdGenerator
{
    string Get();
    void Set(string correlationId);
}