using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class ExportHoursSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[,]
                {
                    { 121, 0, 2, new DateTime(2024, 11, 4, 23, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 4, 17, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 122, 2, 2, new DateTime(2024, 11, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 5, 6, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 123, 1, 2, new DateTime(2024, 11, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 124, 0, 2, new DateTime(2024, 11, 7, 23, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 7, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 125, 2, 2, new DateTime(2024, 11, 8, 14, 30, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 8, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 126, 1, 3, new DateTime(2024, 11, 4, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 4, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 127, 2, 3, new DateTime(2024, 11, 5, 22, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 5, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 128, 0, 3, new DateTime(2024, 11, 11, 14, 30, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 9, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 129, 2, 3, new DateTime(2024, 11, 7, 21, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 7, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 130, 1, 3, new DateTime(2024, 11, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 131, 0, 2, new DateTime(2024, 11, 11, 23, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 11, 17, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 132, 2, 2, new DateTime(2024, 11, 12, 14, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 12, 6, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 133, 1, 2, new DateTime(2024, 11, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 17, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 134, 0, 2, new DateTime(2024, 11, 14, 23, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 14, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 135, 2, 2, new DateTime(2024, 11, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 15, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 136, 1, 3, new DateTime(2024, 11, 11, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 11, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 137, 2, 3, new DateTime(2024, 11, 12, 22, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 12, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 138, 0, 3, new DateTime(2024, 11, 17, 14, 30, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 17, 6, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 139, 2, 3, new DateTime(2024, 11, 14, 21, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 14, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 140, 1, 3, new DateTime(2024, 11, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 11, 15, 10, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "WorkedHours",
                columns: new[] { "Id", "DateOnly", "EmployeeNumber", "EndTime", "StartTime", "Status" },
                values: new object[,]
                {
                    { 21, new DateOnly(2024, 11, 4), 2, new TimeOnly(23, 0, 0), new TimeOnly(17, 0, 0), 1 },
                    { 22, new DateOnly(2024, 11, 5), 2, new TimeOnly(14, 10, 0), new TimeOnly(6, 0, 0), 1 },
                    { 23, new DateOnly(2024, 11, 10), 2, new TimeOnly(18, 5, 0), new TimeOnly(10, 15, 0), 1 },
                    { 24, new DateOnly(2024, 11, 7), 2, new TimeOnly(23, 15, 0), new TimeOnly(15, 10, 0), 1 },
                    { 25, new DateOnly(2024, 11, 8), 2, new TimeOnly(14, 35, 0), new TimeOnly(6, 35, 0), 1 },
                    { 26, new DateOnly(2024, 11, 4), 3, new TimeOnly(16, 20, 0), new TimeOnly(8, 10, 0), 1 },
                    { 27, new DateOnly(2024, 11, 5), 3, new TimeOnly(22, 15, 0), new TimeOnly(14, 10, 0), 1 },
                    { 28, new DateOnly(2024, 11, 9), 3, new TimeOnly(14, 40, 0), new TimeOnly(6, 35, 0), 1 },
                    { 29, new DateOnly(2024, 11, 7), 3, new TimeOnly(21, 5, 0), new TimeOnly(13, 10, 0), 1 },
                    { 30, new DateOnly(2024, 11, 8), 3, new TimeOnly(18, 15, 0), new TimeOnly(10, 10, 0), 1 },
                    { 31, new DateOnly(2024, 11, 11), 2, new TimeOnly(23, 0, 0), new TimeOnly(17, 15, 0), 1 },
                    { 32, new DateOnly(2024, 11, 12), 2, new TimeOnly(14, 20, 0), new TimeOnly(6, 10, 0), 1 },
                    { 33, new DateOnly(2024, 11, 17), 2, new TimeOnly(18, 10, 0), new TimeOnly(10, 20, 0), 1 },
                    { 34, new DateOnly(2024, 11, 14), 2, new TimeOnly(23, 10, 0), new TimeOnly(15, 15, 0), 1 },
                    { 35, new DateOnly(2024, 11, 15), 2, new TimeOnly(14, 40, 0), new TimeOnly(6, 35, 0), 1 },
                    { 36, new DateOnly(2024, 11, 11), 3, new TimeOnly(16, 30, 0), new TimeOnly(8, 15, 0), 1 },
                    { 37, new DateOnly(2024, 11, 12), 3, new TimeOnly(22, 20, 0), new TimeOnly(14, 15, 0), 1 },
                    { 38, new DateOnly(2024, 11, 17), 3, new TimeOnly(14, 50, 0), new TimeOnly(6, 40, 0), 1 },
                    { 39, new DateOnly(2024, 11, 14), 3, new TimeOnly(21, 15, 0), new TimeOnly(13, 20, 0), 1 },
                    { 40, new DateOnly(2024, 11, 15), 3, new TimeOnly(18, 30, 0), new TimeOnly(10, 20, 0), 1 }
                });

            migrationBuilder.InsertData(
                table: "Breaks",
                columns: new[] { "Id", "EndTime", "StartTime", "WorkedHourId" },
                values: new object[,]
                {
                    { 11, new TimeOnly(18, 0, 0), new TimeOnly(17, 30, 0), 21 },
                    { 12, new TimeOnly(9, 30, 0), new TimeOnly(9, 0, 0), 22 },
                    { 13, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 23 },
                    { 14, new TimeOnly(17, 0, 0), new TimeOnly(16, 30, 0), 24 },
                    { 15, new TimeOnly(7, 30, 0), new TimeOnly(7, 0, 0), 25 },
                    { 16, new TimeOnly(12, 30, 0), new TimeOnly(12, 0, 0), 26 },
                    { 17, new TimeOnly(16, 0, 0), new TimeOnly(15, 30, 0), 27 },
                    { 18, new TimeOnly(9, 0, 0), new TimeOnly(8, 30, 0), 28 },
                    { 19, new TimeOnly(15, 30, 0), new TimeOnly(15, 0, 0), 29 },
                    { 20, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 30 },
                    { 31, new TimeOnly(18, 30, 0), new TimeOnly(18, 0, 0), 31 },
                    { 32, new TimeOnly(9, 30, 0), new TimeOnly(9, 0, 0), 32 },
                    { 33, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 33 },
                    { 34, new TimeOnly(17, 30, 0), new TimeOnly(17, 0, 0), 34 },
                    { 35, new TimeOnly(10, 30, 0), new TimeOnly(10, 0, 0), 35 },
                    { 36, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 36 },
                    { 37, new TimeOnly(15, 30, 0), new TimeOnly(15, 0, 0), 37 },
                    { 38, new TimeOnly(9, 0, 0), new TimeOnly(8, 30, 0), 38 },
                    { 39, new TimeOnly(16, 30, 0), new TimeOnly(16, 0, 0), 39 },
                    { 40, new TimeOnly(13, 30, 0), new TimeOnly(13, 0, 0), 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Breaks",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "WorkedHours",
                keyColumn: "Id",
                keyValue: 40);
        }
    }
}
