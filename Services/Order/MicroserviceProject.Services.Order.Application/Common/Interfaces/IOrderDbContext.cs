using MicroserviceProject.Services.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceProject.Services.Order.Application.Common.Interfaces;

public interface IOrderDbContext
{
    DbSet<Domain.Entities.Order> Orders { get; set; }

    DbSet<OrderItem> OrderItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}