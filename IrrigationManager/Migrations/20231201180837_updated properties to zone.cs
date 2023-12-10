using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IrrigationManager.Migrations
{
    /// <inheritdoc />
    public partial class updatedpropertiestozone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Zones");

            migrationBuilder.AddColumn<int>(
                name: "EndHours",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndMinutes",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarHours",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarMinutes",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHours",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "EndMinutes",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "StarHours",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "StarMinutes",
                table: "Zones");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Zones",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Zones",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
