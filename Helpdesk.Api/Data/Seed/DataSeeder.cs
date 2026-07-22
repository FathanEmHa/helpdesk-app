using Helpdesk.Data;
using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk;

public static class DataSeeder
{
    public static async Task SeedAdminAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Role == Role.Admin))
            return;

        var admin = new User
        {
            Name = "Administrator",
            Email = "admin@helpdesk.com",
            PhoneNumber = "08123456789",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = Role.Admin
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}