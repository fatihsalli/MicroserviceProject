using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.ToTable("Orders", OrderDbContext.DEFAULT_SCHEMA);

        builder.OwnsOne(o => o.Address).WithOwner();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdadetAt).IsRequired();
        builder.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");

        builder.Ignore(x => x.DomainEvents);
    }
}