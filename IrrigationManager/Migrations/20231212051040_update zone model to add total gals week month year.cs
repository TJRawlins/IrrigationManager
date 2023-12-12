using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class updatezonemodeltoaddtotalgalsweekmonthyear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalGallons",
                table: "Zones",
                newName: "TotalGalPerYear");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGalPerMonth",
                table: "Zones",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGalPerWeek",
                table: "Zones",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGalPerMonth",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "TotalGalPerWeek",
                table: "Zones");

            migrationBuilder.RenameColumn(
                name: "TotalGalPerYear",
                table: "Zones",
                newName: "TotalGallons");
        }
    }
}
