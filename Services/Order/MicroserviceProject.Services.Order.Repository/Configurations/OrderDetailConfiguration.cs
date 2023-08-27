using MicroserviceProject.Services.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceProject.Services.Order.Repository.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", OrderDbContext.DEFAULT_SCHEMA);
        
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdadetAt).IsRequired();
        
    }
}