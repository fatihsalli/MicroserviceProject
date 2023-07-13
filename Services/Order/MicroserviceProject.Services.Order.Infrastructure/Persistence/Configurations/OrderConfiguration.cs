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
        builder.Property(x => x.CreatedAt).IsRequired(true);
        builder.Property(x => x.UpdadetAt).IsRequired(true);
        builder.Property(x => x.GetTotalPrice).IsRequired(true).HasColumnType("decimal(18,2)");

        builder.Ignore(x => x.DomainEvents);
    }
}