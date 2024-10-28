using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedPrognosisData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prognoses_WeekPrognoses_WeekPrognosisId",
                table: "Prognoses");

            migrationBuilder.AlterColumn<int>(
                name: "WeekPrognosisId",
                table: "Prognoses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Department",
                table: "Prognoses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.InsertData(
                table: "WeekPrognoses",
                columns: new[] { "Id", "StartDate" },
                values: new object[] { 1, new DateOnly(2024, 10, 7) });

            migrationBuilder.InsertData(
                table: "Prognoses",
                columns: new[] { "Id", "Date", "Department", "NeededEmployees", "NeededHours", "WeekPrognosisId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 10, 7), 0, 5f, 40f, 1 },
                    { 2, new DateOnly(2024, 10, 7), 1, 4f, 35f, 1 },
                    { 3, new DateOnly(2024, 10, 7), 2, 3f, 30f, 1 },
                    { 4, new DateOnly(2024, 10, 8), 0, 4f, 38f, 1 },
                    { 5, new DateOnly(2024, 10, 8), 1, 3f, 37f, 1 },
                    { 6, new DateOnly(2024, 10, 8), 2, 2f, 28f, 1 },
                    { 7, new DateOnly(2024, 10, 9), 0, 6f, 42f, 1 },
                    { 8, new DateOnly(2024, 10, 9), 1, 3f, 32f, 1 },
                    { 9, new DateOnly(2024, 10, 9), 2, 2f, 26f, 1 },
                    { 10, new DateOnly(2024, 10, 10), 0, 6f, 42f, 1 },
                    { 11, new DateOnly(2024, 10, 10), 1, 3f, 32f, 1 },
                    { 12, new DateOnly(2024, 10, 10), 2, 2f, 26f, 1 },
                    { 13, new DateOnly(2024, 10, 11), 0, 6f, 42f, 1 },
                    { 14, new DateOnly(2024, 10, 11), 1, 3f, 32f, 1 },
                    { 15, new DateOnly(2024, 10, 11), 2, 2f, 26f, 1 },
                    { 16, new DateOnly(2024, 10, 12), 0, 6f, 42f, 1 },
                    { 17, new DateOnly(2024, 10, 12), 1, 3f, 32f, 1 },
                    { 18, new DateOnly(2024, 10, 12), 2, 2f, 26f, 1 },
                    { 19, new DateOnly(2024, 10, 13), 0, 6f, 42f, 1 },
                    { 20, new DateOnly(2024, 10, 13), 1, 3f, 32f, 1 },
                    { 21, new DateOnly(2024, 10, 13), 2, 2f, 26f, 1 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Prognoses_WeekPrognoses_WeekPrognosisId",
                table: "Prognoses",
                column: "WeekPrognosisId",
                principalTable: "WeekPrognoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prognoses_WeekPrognoses_WeekPrognosisId",
                table: "Prognoses");

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "WeekPrognosisId",
                table: "Prognoses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "Prognoses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Prognoses_WeekPrognoses_WeekPrognosisId",
                table: "Prognoses",
                column: "WeekPrognosisId",
                principalTable: "WeekPrognoses",
                principalColumn: "Id");
        }
    }
}
