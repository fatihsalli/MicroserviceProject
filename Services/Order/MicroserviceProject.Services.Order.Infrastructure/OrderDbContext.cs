using MicroserviceProject.Services.Order.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Infrastructure;

public class OrderDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "Ordering";

    public OrderDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

        modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType(("decimal(18,2)"));
        modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner();

        base.OnModelCreating(modelBuilder);
    }
}

// dotnet ef migrations add MyFirstMigration --startup-project ../MicroserviceProject.Services.Order.API
// dotnet ef database update --startup-project ../MicroserviceProject.Services.Order.API