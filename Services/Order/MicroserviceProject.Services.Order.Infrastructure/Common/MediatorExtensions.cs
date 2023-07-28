using MediatR;
using MicroserviceProject.Services.Order.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Infrastructure.Common;

public static class MediatorExtensions
{
    /// <summary>
    ///     Entity içinde yer alan DomainEvents listelerimizi alarak publish ediyoruz. Publish etmeden öncede DomainEvent'i
    ///     boşaltıyoruz ki tekrar aynı event'leri publish etmemek için. Bu metodu "SaveChangesAsync" metodu çağrıldığında
    ///     kullanmak için "interceptor" metot olarak "OrderDbContext" tarafında yazıldı. "ClearDomainEvents" ile event
    ///     listemizi temizledikten sonra "SaveChangesAsync" metodu çalışacağı için bu sayede bu değişiklik de kaydedilmiş
    ///     olacaktır. Event nesneleri publish edildikten sonra ilgili handlerlar çalışır o sebeple publish edilmeyen event
    ///     herhangi bir handle'da işlenmez.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="context"></param>
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}