using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceProject.Services.Order.Repository.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.ToTable("Orders", OrderDbContext.DEFAULT_SCHEMA);

        // Address özelliklerini belirleyip ve veritabanında saklanması için.
        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Province).IsRequired().HasMaxLength(50);
            address.Property(a => a.District).IsRequired().HasMaxLength(50);
            address.Property(a => a.Street).IsRequired().HasMaxLength(50);
            address.Property(a => a.Zip).IsRequired().HasMaxLength(10);
            address.Property(a => a.Line).IsRequired().HasMaxLength(100);
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.StatusId).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdadetAt).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(250);
        
        builder.Ignore(x => x.DomainEvents);
    }
}