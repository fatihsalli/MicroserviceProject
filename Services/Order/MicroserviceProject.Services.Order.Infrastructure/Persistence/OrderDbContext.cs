using System.Reflection;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Domain.Common;
using MicroserviceProject.Services.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence;

public class OrderDbContext : DbContext, IOrderDbContext
{
    public const string DEFAULT_SCHEMA = "ordering";
    private readonly IDomainEventService _domainEventService;

    public OrderDbContext(DbContextOptions options, IDomainEventService domainEventService) : base(options)
    {
        _domainEventService = domainEventService;
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.UpdadetAt = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdadetAt = DateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}

// dotnet ef migrations add MyFirstMigration --startup-project ../MicroserviceProject.Services.Order.API
// dotnet ef database update --startup-project ../MicroserviceProject.Services.Order.API