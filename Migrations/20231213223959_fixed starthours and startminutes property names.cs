using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class fixedstarthoursandstartminutespropertynames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StarMinutes",
                table: "Zones",
                newName: "StartMinutes");

            migrationBuilder.RenameColumn(
                name: "StarHours",
                table: "Zones",
                newName: "StartHours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartMinutes",
                table: "Zones",
                newName: "StarMinutes");

            migrationBuilder.RenameColumn(
                name: "StartHours",
                table: "Zones",
                newName: "StarHours");
        }
    }
}
