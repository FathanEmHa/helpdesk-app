using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Helpdesk.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Email", "Name", "PasswordHash", "PhoneNumber", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "admin@helpdesk.lokal", "Administrator", "$2a$11$kN7oAFekSN.gHJ9z97vdi.DWM3LOImaSKB8NloyEAB497EDNgqkVe", "-", "Admin", "Active", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
