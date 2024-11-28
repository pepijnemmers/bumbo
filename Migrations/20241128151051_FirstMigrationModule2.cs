using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigrationModule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expectations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedCustomers = table.Column<int>(type: "int", nullable: false),
                    ExpectedCargo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expectations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Norms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    NormType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Norms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    WeekDay = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ClosingTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.WeekDay);
                    table.CheckConstraint("CK_OpeningHours_OpeningTime_ClosingTime", "([OpeningTime] IS NULL AND [ClosingTime] IS NULL) OR ([OpeningTime] IS NOT NULL AND [ClosingTime] IS NOT NULL AND [OpeningTime] < [ClosingTime])");
                });

            migrationBuilder.CreateTable(
                name: "UniqueDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Factor = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueDays", x => x.Id);
                    table.CheckConstraint("CK_UniqueDays_StartDate_EndDate", "[StartDate] <= [EndDate]");
                });

            migrationBuilder.CreateTable(
                name: "WeekPrognoses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekPrognoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContractHours = table.Column<int>(type: "int", nullable: false),
                    LeaveHours = table.Column<int>(type: "int", nullable: false),
                    StartOfEmployment = table.Column<DateOnly>(type: "date", nullable: false),
                    EndOfEmployment = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeNumber);
                    table.CheckConstraint("CK_Employees_StartOfEmployment_EndOfEmployment", "[StartOfEmployment] <= [EndOfEmployment]");
                    table.CheckConstraint("CK_Employees_Zipcode", "[Zipcode] LIKE '[1-9][0-9][0-9][0-9][A-Z][A-Z]'");
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prognoses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekPrognosisId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false),
                    NeededHours = table.Column<float>(type: "real", nullable: false),
                    NeededEmployees = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prognoses", x => x.Id);
                    table.CheckConstraint("CK_Prognoses_NeededHours_NeededEmployees", "[NeededHours] = [NeededEmployees] * 8");
                    table.ForeignKey(
                        name: "FK_Prognoses_WeekPrognoses_WeekPrognosisId",
                        column: x => x.WeekPrognosisId,
                        principalTable: "WeekPrognoses",
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
                    ActionUrl = table.Column<string>(type: "text", nullable: true)
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
                    EmployeeNumber = table.Column<int>(type: "int", nullable: true),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.CheckConstraint("CK_Shifts_Start_End", "[Start] < [End]");
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeNumber",
                        column: x => x.EmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber");
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
                    EmployeeTakingOverEmployeeNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTakeOvers", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_ShiftTakeOvers_Employees_EmployeeTakingOverEmployeeNumber",
                        column: x => x.EmployeeTakingOverEmployeeNumber,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber");
                    table.ForeignKey(
                        name: "FK_ShiftTakeOvers_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", null, "Employee", "EMPLOYEE" },
                    { "dc065cdc-e1d7-4202-936a-fbf03070c74d", null, "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "12544476-38da-4113-9c40-4bc508f8c0f2", 0, "static-concurrency-stamp", "jane.smith@example.com", true, false, null, "JANE.SMITH@EXAMPLE.COM", "JANE.SMITH@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGk4lj3QRvRZzy4Oas9sTTW0A2nJ1X41eB0uiNnGNFQT7RdiOs/FLSjxWz/x4KDk+w==", null, false, "static-security-stamp", false, "jane.smith@example.com" },
                    { "2667ab01-7225-451b-adbb-c99eea968d02", 0, "static-concurrency-stamp", "emily.jones@example.com", true, false, null, "EMILY.JONES@EXAMPLE.COM", "EMILY.JONES@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw==", null, false, "static-security-stamp", false, "emily.jones@example.com" },
                    { "2ab03136-c316-4b70-a7fc-4c9cb044a6be", 0, "static-concurrency-stamp", "john.doe@example.com", true, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHN.DOE@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHElifiD+iCmgFS/WCucV8tMzAcHwDdy1B4kwXCYsxB7xOwvRsxjkQbdJ6YrI77xDA==", null, false, "static-security-stamp", false, "john.doe@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Expectations",
                columns: new[] { "Id", "Date", "ExpectedCargo", "ExpectedCustomers" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 11, 18), 30, 800 },
                    { 2, new DateOnly(2024, 11, 19), 40, 900 },
                    { 3, new DateOnly(2024, 11, 20), 50, 950 },
                    { 4, new DateOnly(2024, 11, 21), 60, 1000 },
                    { 5, new DateOnly(2024, 11, 22), 45, 850 },
                    { 6, new DateOnly(2024, 11, 23), 38, 780 },
                    { 7, new DateOnly(2024, 11, 24), 55, 960 },
                    { 8, new DateOnly(2024, 11, 25), 35, 810 },
                    { 9, new DateOnly(2024, 11, 26), 50, 900 },
                    { 10, new DateOnly(2024, 11, 27), 42, 850 },
                    { 11, new DateOnly(2024, 11, 28), 60, 1000 },
                    { 12, new DateOnly(2024, 11, 29), 37, 820 },
                    { 13, new DateOnly(2024, 11, 30), 53, 940 },
                    { 14, new DateOnly(2024, 12, 1), 50, 900 },
                    { 15, new DateOnly(2024, 12, 2), 36, 810 },
                    { 16, new DateOnly(2024, 12, 3), 47, 870 },
                    { 17, new DateOnly(2024, 12, 4), 38, 780 },
                    { 18, new DateOnly(2024, 12, 5), 55, 950 },
                    { 19, new DateOnly(2024, 12, 6), 45, 840 },
                    { 20, new DateOnly(2024, 12, 7), 60, 1000 },
                    { 21, new DateOnly(2024, 12, 8), 40, 890 },
                    { 22, new DateOnly(2024, 12, 9), 50, 920 },
                    { 23, new DateOnly(2024, 12, 10), 42, 860 },
                    { 24, new DateOnly(2024, 12, 11), 60, 1000 },
                    { 25, new DateOnly(2024, 12, 12), 48, 930 },
                    { 26, new DateOnly(2024, 12, 13), 35, 780 },
                    { 27, new DateOnly(2024, 12, 14), 53, 950 },
                    { 28, new DateOnly(2024, 12, 15), 50, 900 }
                });

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

            migrationBuilder.InsertData(
                table: "OpeningHours",
                columns: new[] { "WeekDay", "ClosingTime", "OpeningTime" },
                values: new object[,]
                {
                    { 0, null, null },
                    { 1, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 2, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 3, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 4, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 5, new TimeOnly(21, 0, 0), new TimeOnly(8, 0, 0) },
                    { 6, new TimeOnly(20, 0, 0), new TimeOnly(9, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "UniqueDays",
                columns: new[] { "Id", "EndDate", "Factor", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 11, 22), 1.25f, "Customer Appreciation Day", new DateOnly(2024, 11, 22) },
                    { 2, new DateOnly(2024, 11, 29), 1.5f, "VIP Shopping Day", new DateOnly(2024, 11, 29) },
                    { 3, new DateOnly(2024, 12, 8), 1.8f, "Weekend Sale", new DateOnly(2024, 12, 7) }
                });

            migrationBuilder.InsertData(
                table: "WeekPrognoses",
                columns: new[] { "Id", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 10, 7) },
                    { 2, new DateOnly(2024, 12, 9) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "12544476-38da-4113-9c40-4bc508f8c0f2" },
                    { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "2667ab01-7225-451b-adbb-c99eea968d02" },
                    { "dc065cdc-e1d7-4202-936a-fbf03070c74d", "2ab03136-c316-4b70-a7fc-4c9cb044a6be" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeNumber", "ContractHours", "DateOfBirth", "EndOfEmployment", "FirstName", "HouseNumber", "LastName", "LeaveHours", "StartOfEmployment", "UserId", "Zipcode" },
                values: new object[,]
                {
                    { 1, 40, new DateOnly(1990, 5, 20), null, "John", "1A", "Doe", 60, new DateOnly(2020, 1, 15), "2ab03136-c316-4b70-a7fc-4c9cb044a6be", "1234AB" },
                    { 2, 20, new DateOnly(1995, 8, 12), null, "Jane", "2B", "Smith", 5, new DateOnly(2021, 3, 1), "12544476-38da-4113-9c40-4bc508f8c0f2", "5684AC" },
                    { 3, 35, new DateOnly(1998, 12, 5), null, "Emily", "3C", "Jones", 40, new DateOnly(2019, 7, 30), "2667ab01-7225-451b-adbb-c99eea968d02", "5211DG" }
                });

            migrationBuilder.InsertData(
                table: "Prognoses",
                columns: new[] { "Id", "Date", "Department", "NeededEmployees", "NeededHours", "WeekPrognosisId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 10, 7), 0, 5f, 40f, 1 },
                    { 2, new DateOnly(2024, 10, 7), 1, 4f, 32f, 1 },
                    { 3, new DateOnly(2024, 10, 7), 2, 3f, 24f, 1 },
                    { 4, new DateOnly(2024, 10, 8), 0, 4f, 32f, 1 },
                    { 5, new DateOnly(2024, 10, 8), 1, 3f, 24f, 1 },
                    { 6, new DateOnly(2024, 10, 8), 2, 2f, 16f, 1 },
                    { 7, new DateOnly(2024, 10, 9), 0, 6f, 48f, 1 },
                    { 8, new DateOnly(2024, 10, 9), 1, 3f, 24f, 1 },
                    { 9, new DateOnly(2024, 10, 9), 2, 2f, 16f, 1 },
                    { 10, new DateOnly(2024, 10, 10), 0, 6f, 48f, 1 },
                    { 11, new DateOnly(2024, 10, 10), 1, 3f, 24f, 1 },
                    { 12, new DateOnly(2024, 10, 10), 2, 2f, 16f, 1 },
                    { 13, new DateOnly(2024, 10, 11), 0, 6f, 48f, 1 },
                    { 14, new DateOnly(2024, 10, 11), 1, 3f, 24f, 1 },
                    { 15, new DateOnly(2024, 10, 11), 2, 2f, 16f, 1 },
                    { 16, new DateOnly(2024, 10, 12), 0, 6f, 48f, 1 },
                    { 17, new DateOnly(2024, 10, 12), 1, 3f, 24f, 1 },
                    { 18, new DateOnly(2024, 10, 12), 2, 2f, 16f, 1 },
                    { 19, new DateOnly(2024, 10, 13), 0, 6f, 48f, 1 },
                    { 20, new DateOnly(2024, 10, 13), 1, 3f, 24f, 1 },
                    { 21, new DateOnly(2024, 10, 13), 2, 2f, 16f, 1 },
                    { 22, new DateOnly(2024, 12, 9), 0, 5f, 40f, 2 },
                    { 23, new DateOnly(2024, 12, 9), 1, 4.5f, 36f, 2 },
                    { 24, new DateOnly(2024, 12, 9), 2, 2.5f, 20f, 2 },
                    { 25, new DateOnly(2024, 12, 10), 0, 4f, 32f, 2 },
                    { 26, new DateOnly(2024, 12, 10), 1, 3.5f, 28f, 2 },
                    { 27, new DateOnly(2024, 12, 10), 2, 2f, 16f, 2 },
                    { 28, new DateOnly(2024, 12, 11), 0, 5f, 40f, 2 },
                    { 29, new DateOnly(2024, 12, 11), 1, 3f, 24f, 2 },
                    { 30, new DateOnly(2024, 12, 11), 2, 2.5f, 20f, 2 },
                    { 31, new DateOnly(2024, 12, 12), 0, 6f, 48f, 2 },
                    { 32, new DateOnly(2024, 12, 12), 1, 4.5f, 36f, 2 },
                    { 33, new DateOnly(2024, 12, 12), 2, 2f, 16f, 2 },
                    { 34, new DateOnly(2024, 12, 13), 0, 5f, 40f, 2 },
                    { 35, new DateOnly(2024, 12, 13), 1, 3.5f, 28f, 2 },
                    { 36, new DateOnly(2024, 12, 13), 2, 1.5f, 12f, 2 },
                    { 37, new DateOnly(2024, 12, 14), 0, 4f, 32f, 2 },
                    { 38, new DateOnly(2024, 12, 14), 1, 5f, 40f, 2 },
                    { 39, new DateOnly(2024, 12, 14), 2, 3f, 24f, 2 },
                    { 40, new DateOnly(2024, 12, 15), 0, 6f, 48f, 2 },
                    { 41, new DateOnly(2024, 12, 15), 1, 4f, 32f, 2 },
                    { 42, new DateOnly(2024, 12, 15), 2, 2f, 16f, 2 }
                });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 9), 1, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 10), 2, new TimeOnly(17, 0, 0), new TimeOnly(13, 0, 0) },
                    { new DateOnly(2024, 12, 11), 3, new TimeOnly(14, 0, 0), new TimeOnly(8, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "EmployeeNumber", "EndDate", "Reason", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family event", new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical appointment", new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "ActionUrl", "Description", "EmployeeNumber", "HasBeenRead", "SentAt", "Title" },
                values: new object[,]
                {
                    { 1, null, "Don't forget the department meeting on Dec 10.", 1, false, new DateTime(2024, 12, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), "Meeting Reminder" },
                    { 2, null, "Check your holiday hours for December.", 3, true, new DateTime(2024, 12, 7, 15, 30, 0, 0, DateTimeKind.Unspecified), "Holiday Hours" }
                });

            migrationBuilder.InsertData(
                table: "SchoolSchedules",
                columns: new[] { "Date", "EmployeeNumber", "DurationInHours" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 9), 1, 4f },
                    { new DateOnly(2024, 12, 10), 1, 4f },
                    { new DateOnly(2024, 12, 9), 2, 3f },
                    { new DateOnly(2024, 12, 10), 2, 3f },
                    { new DateOnly(2024, 12, 9), 3, 6f },
                    { new DateOnly(2024, 12, 10), 3, 6f }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2024, 12, 9, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 12, 9, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 2, new DateTime(2024, 12, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 12, 10, 13, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ShiftTakeOvers",
                columns: new[] { "ShiftId", "EmployeeTakingOverEmployeeNumber", "Status" },
                values: new object[,]
                {
                    { 1, 2, 2 },
                    { 2, 3, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expectations_Date",
                table: "Expectations",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeNumber",
                table: "LeaveRequests",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeNumber",
                table: "Notifications",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Prognoses_Date_Department",
                table: "Prognoses",
                columns: new[] { "Date", "Department" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prognoses_WeekPrognosisId",
                table: "Prognoses",
                column: "WeekPrognosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeNumber",
                table: "Shifts",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftTakeOvers_EmployeeTakingOverEmployeeNumber",
                table: "ShiftTakeOvers",
                column: "EmployeeTakingOverEmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_WeekPrognoses_StartDate",
                table: "WeekPrognoses",
                column: "StartDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropTable(
                name: "Expectations");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Norms");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "Prognoses");

            migrationBuilder.DropTable(
                name: "SchoolSchedules");

            migrationBuilder.DropTable(
                name: "ShiftTakeOvers");

            migrationBuilder.DropTable(
                name: "SickLeaves");

            migrationBuilder.DropTable(
                name: "UniqueDays");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "WeekPrognoses");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
