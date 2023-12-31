﻿using MicroserviceProject.Services.Order.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MicroserviceProject.Services.Order.Repository.Interceptors;

/// <summary>
///     SavingChanges veya SavingChangesAsync metotları için "interceptor" metotlarımızı oluşturuyoruz. Amacımız
///     "CreatedAt" veya "UpdatedAt" gibi değerleri business katmanında tek tek vermek yerine daha merkezi bir noktadan
///     kontrol etmek.
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.UpdadetAt = entry.Entity.CreatedAt;
            }

            if (entry.State == EntityState.Modified || Extensions.HasChangedOwnedEntities(entry))
            {
                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Entity.UpdadetAt = DateTime.Now;
            }
        }
    }
}

public static class Extensions
{
    /// <summary>
    ///     Owned varlık nesnelerinde herhangi bir değişim olup olmadığını kontrol etmek için kullanılır. Örneğin "Order"
    ///     modelinde sadece value object olan "Address" kısmında bir değişiklik olduğu taktirde bu metot sayesinde
    ///     "UpdateEntities" metodunda bu durumu yakalayabiliyoruz.
    /// </summary>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}