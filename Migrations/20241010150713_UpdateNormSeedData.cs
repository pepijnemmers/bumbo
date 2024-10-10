using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNormSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Activity",
                table: "Norms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Norms",
                columns: new[] { "Id", "Activity", "CreatedAt", "NormType", "Value" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 0, 5 },
                    { 2, 1, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 30 },
                    { 3, 2, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, 30 },
                    { 4, 3, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, 100 },
                    { 5, 4, new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 3, 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Activity",
                table: "Norms");
        }
    }
}
