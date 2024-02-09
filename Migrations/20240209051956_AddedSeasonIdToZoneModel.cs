using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeasonIdToZoneModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "Zones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Season_SeasonId",
                table: "Zones",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id");
        }
    }
}
