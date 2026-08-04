using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.DeletedAt == null);

        modelBuilder.Entity<Ticket>()
            .HasQueryFilter(t => t.DeletedAt == null);

        modelBuilder.Entity<Comment>()
            .HasQueryFilter(c => c.DeletedAt == null);

        base.OnModelCreating(modelBuilder);
    }
}