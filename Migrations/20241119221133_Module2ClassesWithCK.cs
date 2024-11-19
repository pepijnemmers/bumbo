using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class Module2ClassesWithCK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContractHours = table.Column<int>(type: "int", nullable: false),
                    StartOfEmployment = table.Column<DateOnly>(type: "date", nullable: false),
                    EndOfEmployment = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeNumber);
                    table.CheckConstraint("CK_Employees_StartOfEmployment_EndOfEmployment", "[StartOfEmployment] <= [EndOfEmployment]");
                    table.CheckConstraint("CK_Employees_Zipcode", "[Zipcode] LIKE '[1-9][0-9][0-9][0-9][A-Z][A-Z]'");
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => new { x.EmployeeNumber, x.Date });
                    table.CheckConstraint("CK_Availability_StartTime_EndTime", "[StartTime] < [EndTime]");
                    table.ForeignKey(
                        name: "FK_Availabilities_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.CheckConstraint("CK_LeaveRequests_StartDate_EndDate", "[StartDate] <= [EndDate]");
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasBeenRead = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSchedules",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DurationInHours = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSchedules", x => new { x.EmployeeNumber, x.Date });
                    table.ForeignKey(
                        name: "FK_SchoolSchedules_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.CheckConstraint("CK_Shifts_Start_End", "[Start] < [End]");
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SickLeaves",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SickLeaves", x => new { x.EmployeeNumber, x.Date });
                    table.ForeignKey(
                        name: "FK_SickLeaves_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftTakeOvers",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    EmployeeTakingOverEmployeeNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTakeOvers", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                        column: x => x.EmployeeTakingOverEmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftTakeOvers_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeekPrognoses_StartDate",
                table: "WeekPrognoses",
                column: "StartDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_UniqueDays_StartDate_EndDate",
                table: "UniqueDays",
                sql: "[StartDate] <= [EndDate]");

            migrationBuilder.CreateIndex(
                name: "IX_Prognoses_Date_Department",
                table: "Prognoses",
                columns: new[] { "Date", "Department" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Prognoses_NeededHours_NeededEmployees",
                table: "Prognoses",
                sql: "[NeededHours] = [NeededEmployees] * 8");

            migrationBuilder.AddCheckConstraint(
                name: "CK_OpeningHours_OpeningTime_ClosingTime",
                table: "OpeningHours",
                sql: "([OpeningTime] IS NULL AND [ClosingTime] IS NULL) OR ([OpeningTime] IS NOT NULL AND [ClosingTime] IS NOT NULL AND [OpeningTime] < [ClosingTime])");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeNumber",
                table: "LeaveRequests",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeNumber",
                table: "Notifications",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeNumber",
                table: "Shifts",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftTakeOvers_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                column: "EmployeeTakingOverEmployeeNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SchoolSchedules");

            migrationBuilder.DropTable(
                name: "ShiftTakeOvers");

            migrationBuilder.DropTable(
                name: "SickLeaves");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_WeekPrognoses_StartDate",
                table: "WeekPrognoses");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UniqueDays_StartDate_EndDate",
                table: "UniqueDays");

            migrationBuilder.DropIndex(
                name: "IX_Prognoses_Date_Department",
                table: "Prognoses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Prognoses_NeededHours_NeededEmployees",
                table: "Prognoses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_OpeningHours_OpeningTime_ClosingTime",
                table: "OpeningHours");
        }
    }
}
