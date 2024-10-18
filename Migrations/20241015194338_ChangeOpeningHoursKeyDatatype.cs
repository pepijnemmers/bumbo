using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOpeningHoursKeyDatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
            name: "PK_OpeningHours",
            table: "OpeningHours");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpeningTime",
                table: "OpeningHours",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ClosingTime",
                table: "OpeningHours",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<int>(
                name: "WeekDay",
                table: "OpeningHours",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
            name: "PK_OpeningHours",
            table: "OpeningHours",
            column: "WeekDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
            name: "PK_OpeningHours",
            table: "OpeningHours");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpeningTime",
                table: "OpeningHours",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ClosingTime",
                table: "OpeningHours",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WeekDay",
                table: "OpeningHours",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
            name: "PK_OpeningHours",
            table: "OpeningHours",
            column: "WeekDay");
        }
    }
}
