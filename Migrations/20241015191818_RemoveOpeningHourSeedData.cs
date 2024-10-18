using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOpeningHourSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Friday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Monday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Saturday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Sunday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Thursday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Tuesday");

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: "Wednesday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OpeningHours",
                columns: new[] { "WeekDay", "ClosingTime", "OpeningTime" },
                values: new object[,]
                {
                    { "Friday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) },
                    { "Monday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) },
                    { "Saturday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) },
                    { "Sunday", new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { "Thursday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) },
                    { "Tuesday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) },
                    { "Wednesday", new TimeOnly(21, 0, 0), new TimeOnly(7, 0, 0) }
                });
        }
    }
}
