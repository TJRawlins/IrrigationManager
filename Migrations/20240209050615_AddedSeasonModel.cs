using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeasonModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Zones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    TimeStamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGalPerWeek = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    TotalGalPerMonth = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    TotalGalPerYear = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    TotalZones = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_SeasonId",
                table: "Zones",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropIndex(
                name: "IX_Zones_SeasonId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Zones");
        }
    }
}
