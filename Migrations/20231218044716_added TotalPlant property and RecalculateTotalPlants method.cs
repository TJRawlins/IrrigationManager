using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class addedTotalPlantpropertyandRecalculateTotalPlantsmethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPlants",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPlants",
                table: "Zones");
        }
    }
}
