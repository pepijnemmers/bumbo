using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class ImprovedAvailabilitySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 1 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 11), 3 });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 2), 2, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 3), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 4), 2, new TimeOnly(15, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2024, 12, 5), 2, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { new DateOnly(2024, 12, 6), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2024, 12, 7), 2, new TimeOnly(14, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 8), 2, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 9), 2, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2024, 12, 11), 2, new TimeOnly(15, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 12), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2024, 12, 13), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 14), 2, new TimeOnly(13, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 15), 2, new TimeOnly(17, 0, 0), new TimeOnly(11, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 2), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 3), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 4), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 5), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 6), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 7), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 8), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 11), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 12), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 13), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 14), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 15), 2 });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 9), 1, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 11), 3, new TimeOnly(14, 0, 0), new TimeOnly(8, 0, 0) }
                });
        }
    }
}
