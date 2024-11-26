using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class ShiftAndEmployeeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeNumber",
                table: "Shifts");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNumber",
                table: "Shifts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LeaveHours",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 1,
                column: "LeaveHours",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 2,
                column: "LeaveHours",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3,
                column: "LeaveHours",
                value: 40);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeNumber",
                table: "Shifts",
                column: "EmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeNumber",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "LeaveHours",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNumber",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeNumber",
                table: "Shifts",
                column: "EmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
