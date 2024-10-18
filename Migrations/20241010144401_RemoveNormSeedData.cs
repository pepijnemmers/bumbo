using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNormSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
