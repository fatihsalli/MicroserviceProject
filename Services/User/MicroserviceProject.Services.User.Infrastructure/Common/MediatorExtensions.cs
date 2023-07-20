using MediatR;
using MicroserviceProject.Services.User.Application.Common.Interfaces;
using MicroserviceProject.Services.User.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Infrastructure.Common;

public static class MediatorExtensions
{
    /// <summary>
    /// Order mikroservisinde bu metodu kullanırken "Interceptor" kullanarak SaveChanges metodu çalıştırıldığında araya girerek context üzerinden "ChangeTracker" ile "DomainEvents" listesine ulaşabiliyorduk. Ancak MongoDb tarafında "Interceptor" mantığı olmadığı için User modelimizi direkt olarak almak durumunda kaldık. Çünkü "DomainEvents" listemizi herhangi bir database'e kaydetmiyoruz. Publish etmek için biriktiriyoruz sadece.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="user"></param>
    public static async Task DispatchDomainEvents(this IMediator mediator, Domain.Entities.User user) 
    {
        foreach (var domainEvent in user.DomainEvents)
            await mediator.Publish(domainEvent);
        
        user.ClearDomainEvents();
    }
}



