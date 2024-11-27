using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "UniqueDays",
                columns: new[] { "Id", "EndDate", "Factor", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 11, 22), 1.25f, "Customer Appreciation Day", new DateOnly(2024, 11, 22) },
                    { 2, new DateOnly(2024, 11, 29), 1.5f, "VIP Shopping Day", new DateOnly(2024, 11, 29) },
                    { 3, new DateOnly(2024, 12, 8), 1.8f, "Weekend Sale", new DateOnly(2024, 12, 7) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "qwer1234", 1 },
                    { 2, "jane.smith@example.com", "asdf1234", 0 },
                    { 3, "emily.jones@example.com", "zxcv1234", 0 }
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
                table: "Employees",
                columns: new[] { "EmployeeNumber", "ContractHours", "DateOfBirth", "EndOfEmployment", "FirstName", "HouseNumber", "LastName", "StartOfEmployment", "UserId", "Zipcode" },
                values: new object[,]
                {
                    { 1, 40, new DateOnly(1990, 5, 20), null, "John", "1A", "Doe", new DateOnly(2020, 1, 15), 1, "1234AB" },
                    { 2, 20, new DateOnly(1995, 8, 12), null, "Jane", "2B", "Smith", new DateOnly(2021, 3, 1), 2, "5684AC" },
                    { 3, 35, new DateOnly(1998, 12, 5), null, "Emily", "3C", "Jones", new DateOnly(2019, 7, 30), 3, "5211DG" }
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
                columns: new[] { "Id", "Description", "EmployeeNumber", "HasBeenRead", "SentAt", "Text", "Title" },
                values: new object[,]
                {
                    { 1, "Don't forget the department meeting on Dec 10.", 1, false, new DateTime(2024, 12, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "Meeting Reminder" },
                    { 2, "Check your holiday hours for December.", 3, true, new DateTime(2024, 12, 7, 15, 30, 0, 0, DateTimeKind.Unspecified), null, "Holiday Hours" }
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
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "Start" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2024, 12, 9, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 2, new DateTime(2024, 12, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 13, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ShiftTakeOvers",
                columns: new[] { "ShiftId", "EmployeeTakingOverEmployeeNumber" },
                values: new object[] { 2, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 1 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 11), 3 });

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
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2);

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
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2);

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
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 1 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 1 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 3 });

            migrationBuilder.DeleteData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 1);

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

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
