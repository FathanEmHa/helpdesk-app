using Helpdesk.Models;
using Helpdesk.Services;
using Helpdesk.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Data;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserAccessor _currentUser;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserAccessor currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.CreatedBy = _currentUser.UserId;

                    break;

                case EntityState.Modified:

                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;

                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = _currentUser.UserId;

                    break;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        // =========================
        // Optimistic Concurrency
        // =========================

        modelBuilder.Entity<User>()
            .Property(u => u.Version)
            .IsRowVersion();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Version)
            .IsRowVersion();

        modelBuilder.Entity<Comment>()
            .Property(c => c.Version)
            .IsRowVersion();

        // =========================
        // Audit Fields
        // =========================

        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Comment>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // =========================
        // Soft Delete
        // =========================

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.DeletedAt == null);

        modelBuilder.Entity<Ticket>()
            .HasQueryFilter(t => t.DeletedAt == null);

        modelBuilder.Entity<Comment>()
            .HasQueryFilter(c => c.DeletedAt == null);

        base.OnModelCreating(modelBuilder);
    }
}