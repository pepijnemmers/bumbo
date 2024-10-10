using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAllButUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Expectations",
                columns: new[] { "Id", "Date", "ExpectedCargo", "ExpectedCustomers" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 10, 14), 32, 850 },
                    { 2, new DateOnly(2024, 10, 15), 40, 900 },
                    { 3, new DateOnly(2024, 10, 16), 50, 980 },
                    { 4, new DateOnly(2024, 10, 17), 60, 1050 },
                    { 5, new DateOnly(2024, 10, 18), 45, 870 },
                    { 6, new DateOnly(2024, 10, 19), 38, 810 },
                    { 7, new DateOnly(2024, 10, 20), 55, 1000 },
                    { 8, new DateOnly(2024, 10, 21), 33, 830 },
                    { 9, new DateOnly(2024, 10, 22), 48, 920 },
                    { 10, new DateOnly(2024, 10, 23), 42, 880 },
                    { 11, new DateOnly(2024, 10, 24), 60, 1050 },
                    { 12, new DateOnly(2024, 10, 25), 36, 840 },
                    { 13, new DateOnly(2024, 10, 26), 53, 980 },
                    { 14, new DateOnly(2024, 10, 27), 50, 950 },
                    { 15, new DateOnly(2024, 10, 28), 37, 820 },
                    { 16, new DateOnly(2024, 10, 29), 47, 930 },
                    { 17, new DateOnly(2024, 10, 30), 35, 850 },
                    { 18, new DateOnly(2024, 10, 31), 52, 1000 },
                    { 19, new DateOnly(2024, 11, 1), 40, 890 },
                    { 20, new DateOnly(2024, 11, 2), 60, 1050 },
                    { 21, new DateOnly(2024, 11, 3), 44, 870 }
                });

            migrationBuilder.InsertData(
                table: "Norms",
                columns: new[] { "Id", "CreatedAt", "NormType", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 0, 5 },
                    { 2, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 30 },
                    { 3, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, 30 },
                    { 4, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, 100 },
                    { 5, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 3, 30 }
                });

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

            migrationBuilder.InsertData(
                table: "UniqueDays",
                columns: new[] { "Id", "EndDate", "Factor", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 10, 20), 1.25f, "Customer Appreciation Day", new DateOnly(2024, 10, 20) },
                    { 2, new DateOnly(2024, 10, 20), 1.5f, "VIP Shopping Day", new DateOnly(2024, 10, 20) },
                    { 3, new DateOnly(2024, 10, 29), 1.8f, "Weekend Sale", new DateOnly(2024, 10, 28) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Norms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Norms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Norms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Norms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Norms",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.DeleteData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
