using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataModule3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[,]
                {
                    { 101, 0, 2, new DateTime(2024, 12, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 2, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 2, 2, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 1, 2, new DateTime(2024, 12, 4, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 4, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 104, 0, 2, new DateTime(2024, 12, 5, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 5, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 2, 2, new DateTime(2024, 12, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 106, 1, 3, new DateTime(2024, 12, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 2, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 107, 2, 3, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 108, 0, 3, new DateTime(2024, 12, 4, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 4, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 109, 2, 3, new DateTime(2024, 12, 5, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 5, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 110, 1, 3, new DateTime(2024, 12, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 111, 0, 2, new DateTime(2024, 12, 9, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 9, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 112, 2, 2, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 10, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 113, 1, 2, new DateTime(2024, 12, 11, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 11, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 114, 0, 2, new DateTime(2024, 12, 12, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 12, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 115, 2, 2, new DateTime(2024, 12, 13, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 13, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 116, 1, 3, new DateTime(2024, 12, 9, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 9, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 117, 2, 3, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 10, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 118, 0, 3, new DateTime(2024, 12, 11, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 11, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 119, 2, 3, new DateTime(2024, 12, 12, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 12, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 120, 1, 3, new DateTime(2024, 12, 13, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 12, 13, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "WorkedHour",
                columns: new[] { "Id", "DateOnly", "EmployeeNumber", "EndTime", "StartTime", "Status" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 12, 2), 2, new TimeOnly(17, 10, 0), new TimeOnly(9, 15, 0), 1 },
                    { 2, new DateOnly(2024, 12, 3), 2, new TimeOnly(18, 5, 0), new TimeOnly(10, 5, 0), 1 },
                    { 3, new DateOnly(2024, 12, 4), 2, new TimeOnly(16, 5, 0), new TimeOnly(8, 10, 0), 1 },
                    { 4, new DateOnly(2024, 12, 5), 2, new TimeOnly(19, 15, 0), new TimeOnly(11, 10, 0), 1 },
                    { 5, new DateOnly(2024, 12, 6), 2, new TimeOnly(17, 5, 0), new TimeOnly(9, 5, 0), 1 },
                    { 6, new DateOnly(2024, 12, 2), 3, new TimeOnly(17, 20, 0), new TimeOnly(9, 10, 0), 1 },
                    { 7, new DateOnly(2024, 12, 3), 3, new TimeOnly(18, 5, 0), new TimeOnly(10, 0, 0), 1 },
                    { 8, new DateOnly(2024, 12, 4), 3, new TimeOnly(16, 10, 0), new TimeOnly(8, 5, 0), 1 },
                    { 9, new DateOnly(2024, 12, 5), 3, new TimeOnly(19, 0, 0), new TimeOnly(11, 10, 0), 1 },
                    { 10, new DateOnly(2024, 12, 6), 3, new TimeOnly(17, 10, 0), new TimeOnly(9, 0, 0), 1 },
                    { 11, new DateOnly(2024, 12, 9), 2, new TimeOnly(17, 15, 0), new TimeOnly(9, 20, 0), 0 },
                    { 12, new DateOnly(2024, 12, 10), 2, new TimeOnly(18, 10, 0), new TimeOnly(10, 10, 0), 0 },
                    { 13, new DateOnly(2024, 12, 11), 2, new TimeOnly(16, 10, 0), new TimeOnly(8, 20, 0), 0 },
                    { 14, new DateOnly(2024, 12, 12), 2, new TimeOnly(19, 20, 0), new TimeOnly(11, 15, 0), 0 },
                    { 15, new DateOnly(2024, 12, 13), 2, new TimeOnly(17, 10, 0), new TimeOnly(9, 15, 0), 0 },
                    { 16, new DateOnly(2024, 12, 9), 3, new TimeOnly(17, 20, 0), new TimeOnly(9, 15, 0), 0 },
                    { 17, new DateOnly(2024, 12, 10), 3, new TimeOnly(18, 10, 0), new TimeOnly(10, 15, 0), 0 },
                    { 18, new DateOnly(2024, 12, 11), 3, new TimeOnly(16, 15, 0), new TimeOnly(8, 10, 0), 0 },
                    { 19, new DateOnly(2024, 12, 12), 3, new TimeOnly(19, 10, 0), new TimeOnly(11, 20, 0), 0 },
                    { 20, new DateOnly(2024, 12, 13), 3, new TimeOnly(17, 20, 0), new TimeOnly(9, 10, 0), 0 }
                });

            migrationBuilder.InsertData(
                table: "Break",
                columns: new[] { "Id", "EndTime", "StartTime", "WorkedHourId" },
                values: new object[,]
                {
                    { 1, new TimeOnly(12, 30, 0), new TimeOnly(12, 0, 0), 1 },
                    { 2, new TimeOnly(13, 30, 0), new TimeOnly(13, 0, 0), 2 },
                    { 3, new TimeOnly(12, 45, 0), new TimeOnly(12, 15, 0), 3 },
                    { 4, new TimeOnly(15, 0, 0), new TimeOnly(14, 30, 0), 4 },
                    { 5, new TimeOnly(12, 30, 0), new TimeOnly(12, 0, 0), 5 },
                    { 6, new TimeOnly(12, 45, 0), new TimeOnly(12, 15, 0), 6 },
                    { 7, new TimeOnly(14, 0, 0), new TimeOnly(13, 30, 0), 7 },
                    { 8, new TimeOnly(12, 15, 0), new TimeOnly(11, 45, 0), 8 },
                    { 9, new TimeOnly(14, 30, 0), new TimeOnly(14, 0, 0), 9 },
                    { 10, new TimeOnly(12, 45, 0), new TimeOnly(12, 15, 0), 10 },
                    { 21, new TimeOnly(12, 45, 0), new TimeOnly(12, 15, 0), 11 },
                    { 22, new TimeOnly(13, 30, 0), new TimeOnly(13, 0, 0), 12 },
                    { 23, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 13 },
                    { 24, new TimeOnly(14, 30, 0), new TimeOnly(14, 0, 0), 14 },
                    { 25, new TimeOnly(12, 30, 0), new TimeOnly(12, 0, 0), 15 },
                    { 26, new TimeOnly(12, 45, 0), new TimeOnly(12, 15, 0), 16 },
                    { 27, new TimeOnly(13, 45, 0), new TimeOnly(13, 15, 0), 17 },
                    { 28, new TimeOnly(12, 15, 0), new TimeOnly(11, 45, 0), 18 },
                    { 29, new TimeOnly(14, 45, 0), new TimeOnly(14, 15, 0), 19 },
                    { 30, new TimeOnly(13, 0, 0), new TimeOnly(12, 30, 0), 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Break",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "WorkedHour",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
