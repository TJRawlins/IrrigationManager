using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimestampToPlantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "TimeStamp",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Plants");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
