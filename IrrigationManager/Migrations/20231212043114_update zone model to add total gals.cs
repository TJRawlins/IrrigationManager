using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class updatezonemodeltoaddtotalgals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalGallons",
                table: "Zones",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGallons",
                table: "Zones");
        }
    }
}
