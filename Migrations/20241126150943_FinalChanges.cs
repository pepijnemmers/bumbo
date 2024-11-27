using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class FinalChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Notifications",
                newName: "ActionUrl");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ShiftTakeOvers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinal",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 2,
                column: "Status",
                value: 0);

            migrationBuilder.InsertData(
                table: "ShiftTakeOvers",
                columns: new[] { "ShiftId", "EmployeeTakingOverEmployeeNumber", "Status" },
                values: new object[] { 1, 2, 2 });

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsFinal",
                value: false);

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsFinal",
                value: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                column: "EmployeeTakingOverEmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers");

            migrationBuilder.DeleteData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShiftTakeOvers");

            migrationBuilder.DropColumn(
                name: "IsFinal",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "ActionUrl",
                table: "Notifications",
                newName: "Text");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                column: "EmployeeTakingOverEmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
