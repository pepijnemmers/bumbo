using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataEssential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OpeningHours",
                columns: new[] { "WeekDay", "ClosingTime", "OpeningTime" },
                values: new object[,]
                {
                    { 0, null, null },
                    { 1, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 2, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 3, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 4, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 5, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 6, new TimeOnly(20, 0, 0), new TimeOnly(9, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "WeekDay",
                keyValue: 6);
        }
    }
}
