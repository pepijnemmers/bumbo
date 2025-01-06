using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class DbSetAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Break_WorkedHour_WorkedHourId",
                table: "Break");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkedHour_Employees_EmployeeNumber",
                table: "WorkedHour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkedHour",
                table: "WorkedHour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Break",
                table: "Break");

            migrationBuilder.RenameTable(
                name: "WorkedHour",
                newName: "WorkedHours");

            migrationBuilder.RenameTable(
                name: "Break",
                newName: "Breaks");

            migrationBuilder.RenameIndex(
                name: "IX_WorkedHour_EmployeeNumber",
                table: "WorkedHours",
                newName: "IX_WorkedHours_EmployeeNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Break_WorkedHourId",
                table: "Breaks",
                newName: "IX_Breaks_WorkedHourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkedHours",
                table: "WorkedHours",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Breaks_WorkedHours_WorkedHourId",
                table: "Breaks",
                column: "WorkedHourId",
                principalTable: "WorkedHours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkedHours_Employees_EmployeeNumber",
                table: "WorkedHours",
                column: "EmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breaks_WorkedHours_WorkedHourId",
                table: "Breaks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkedHours_Employees_EmployeeNumber",
                table: "WorkedHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkedHours",
                table: "WorkedHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks");

            migrationBuilder.RenameTable(
                name: "WorkedHours",
                newName: "WorkedHour");

            migrationBuilder.RenameTable(
                name: "Breaks",
                newName: "Break");

            migrationBuilder.RenameIndex(
                name: "IX_WorkedHours_EmployeeNumber",
                table: "WorkedHour",
                newName: "IX_WorkedHour_EmployeeNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Breaks_WorkedHourId",
                table: "Break",
                newName: "IX_Break_WorkedHourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkedHour",
                table: "WorkedHour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Break",
                table: "Break",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Break_WorkedHour_WorkedHourId",
                table: "Break",
                column: "WorkedHourId",
                principalTable: "WorkedHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkedHour_Employees_EmployeeNumber",
                table: "WorkedHour",
                column: "EmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
