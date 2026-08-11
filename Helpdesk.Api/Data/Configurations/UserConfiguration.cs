using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Helpdesk.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = 1,
                Name = "Administrator",
                Email = "admin@helpdesk.lokal",

                PasswordHash =
                    "$2a$11$kN7oAFekSN.gHJ9z97vdi.DWM3LOImaSKB8NloyEAB497EDNgqkVe",

                Role = Role.Admin,
                Status = UserStatus.Active,
                PhoneNumber = "-",

                CreatedAt = new DateTime(
                    2026, 8, 11, 0, 0, 0,
                    DateTimeKind.Utc)
            }
        );
    }
}