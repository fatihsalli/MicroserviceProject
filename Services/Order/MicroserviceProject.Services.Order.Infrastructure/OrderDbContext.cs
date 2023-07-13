using MicroserviceProject.Services.Order.Domain.Common;
using MicroserviceProject.Services.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Infrastructure;

public class OrderDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "ordering";

    public OrderDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt=DateTime.Now;
                    entry.Entity.UpdadetAt=DateTime.Now;
                    break;
                    
                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdadetAt=DateTime.Now;
                    break;
            }
        }
        
        
        
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Order>().ToTable("Orders", DEFAULT_SCHEMA);
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

        modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType(("decimal(18,2)"));
        modelBuilder.Entity<Domain.Entities.Order>().OwnsOne(o => o.Address).WithOwner();

        base.OnModelCreating(modelBuilder);
    }
}

// dotnet ef migrations add MyFirstMigration --startup-project ../MicroserviceProject.Services.Order.API
// dotnet ef database update --startup-project ../MicroserviceProject.Services.Order.API