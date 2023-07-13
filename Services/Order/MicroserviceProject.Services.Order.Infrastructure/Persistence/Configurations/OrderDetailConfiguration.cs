using MicroserviceProject.Services.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", OrderDbContext.DEFAULT_SCHEMA);
        builder.Property(x => x.Price).HasColumnType(("decimal(18,2)"));
    }
}