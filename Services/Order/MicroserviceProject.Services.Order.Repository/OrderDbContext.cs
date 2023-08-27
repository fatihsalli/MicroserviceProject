using System.Reflection;
using MediatR;
using MicroserviceProject.Services.Order.Domain.Entities;
using MicroserviceProject.Services.Order.Repository.Common;
using MicroserviceProject.Services.Order.Repository.Interceptors;
using MicroserviceProject.Services.Order.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Repository;

public class OrderDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "ordering";
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    private readonly IMediator _mediator;

    public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    // Eventleri burada publish etmek doğru bir yaklaşım değil. Çünkü daha database'e kaydetme işleminin sorunsuz gerçekleştiğini garanti edemedik. O sebeple de eğer ki database tarafına kaydetme esnasında hata alınırsa eventler publish edilmiş olacaktır. Diğer durumda burada eventler publish edildiğinde ilgili handlerlarda Kafka'ya mesaj gönderecek şekilde senaryo revize edilmiştir.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }

    // Eventleri daha kontrollü şekilde publish edebilmek için bu metot ayrıca yazılmıştır.
    public async Task PublishDomainEvents()
    {
        await _mediator.DispatchDomainEvents(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}

// dotnet ef migrations add MyFirstMigration --startup-project ../MicroserviceProject.Services.Order.API
// dotnet ef database update --startup-project ../MicroserviceProject.Services.Order.API