using FluentValidation;
using MediatR;
using ValidationException = MicroserviceProject.Shared.Exceptions.ValidationException;

namespace MicroserviceProject.Services.Order.Application.Common.Behaviours;


/// <summary>
/// "Mediator" kütüphanesi kullanarak "IPipelineBehaviour" ile birlikte validation hatalarımızı yakalayıp "ValidationException" hatası fırlatıyoruz. ConfigureServices-AddApplicationServices extension metodu ile DI container'a eklenmiştir.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            // Tüm asenkron işlemlerin tamamlanmasını beklemek için => await Task.WhenAll
            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();
            
            if (failures.Any())
                throw new ValidationException(failures);
        }

        return await next();
    }
}