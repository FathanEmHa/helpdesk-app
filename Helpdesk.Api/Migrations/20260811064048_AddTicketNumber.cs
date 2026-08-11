using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Helpdesk.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketNumber",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Tickets"
                SET "TicketNumber" =
                    'TKT-' || LPAD("Id"::text, 6, '0')
                WHERE "TicketNumber" IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "TicketNumber",
                table: "Tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketNumber",
                table: "Tickets",
                column: "TicketNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "Tickets");
        }
    }
}
