using Helpdesk.Models;
using Helpdesk.Services;
using Helpdesk.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // =========================
        // Enum To String
        // =========================

        modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Priority)
            .HasConversion<string>();

        // =========================
        // Unique Index
        // =========================

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.TicketNumber)
            .IsUnique();

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