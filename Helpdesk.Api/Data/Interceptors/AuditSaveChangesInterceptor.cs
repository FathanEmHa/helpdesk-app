using Helpdesk.Models.Base;
using Helpdesk.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Helpdesk.Data.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserAccessor _currentUser;

    public AuditSaveChangesInterceptor(
        ICurrentUserAccessor currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context == null)
            return;

        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker
            .Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = _currentUser.UserId;

                    break;

                case EntityState.Modified:

                    entry.Property(e => e.CreatedAt)
                        .IsModified = false;

                    entry.Property(e => e.CreatedBy)
                        .IsModified = false;

                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = _currentUser.UserId;

                    break;
            }
        }
    }
}