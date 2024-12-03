using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class StandardAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardAvailabilities",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardAvailabilities", x => new { x.Day, x.EmployeeNumber });
                    table.ForeignKey(
                        name: "FK_StandardAvailabilities_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "StandardAvailabilities",
                columns: new[] { "Day", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 0, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 0, 3, new TimeOnly(16, 0, 0), new TimeOnly(12, 0, 0) },
                    { 1, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 1, 3, new TimeOnly(14, 30, 0), new TimeOnly(8, 30, 0) },
                    { 2, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 2, 3, new TimeOnly(19, 0, 0), new TimeOnly(13, 0, 0) },
                    { 3, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 3, 3, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { 4, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 4, 3, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { 5, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 5, 3, new TimeOnly(20, 0, 0), new TimeOnly(14, 0, 0) },
                    { 6, 2, new TimeOnly(21, 0, 0), new TimeOnly(9, 0, 0) },
                    { 6, 3, new TimeOnly(18, 0, 0), new TimeOnly(11, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardAvailabilities_EmployeeNumber",
                table: "StandardAvailabilities",
                column: "EmployeeNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardAvailabilities");
        }
    }
}
