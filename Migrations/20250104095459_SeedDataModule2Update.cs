using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataModule2Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 2), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 3), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 4), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 5), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 6), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 7), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 8), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 11), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 12), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 13), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 14), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 15), 2 });

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
                keyValues: new object[] { new DateOnly(2024, 12, 11), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 12), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 13), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 9), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 10), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 11), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 12), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2024, 12, 13), 3 });

            migrationBuilder.DeleteData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "12544476-38da-4113-9c40-4bc508f8c0f2",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JANE.SMITH@EXAMPLE.COM", "jane.smith@example.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2667ab01-7225-451b-adbb-c99eea968d02",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "EMILY.JONES@EXAMPLE.COM", "emily.jones@example.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2ab03136-c316-4b70-a7fc-4c9cb044a6be",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JOHN.DOE@EXAMPLE.COM", "john.doe@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4b26e441-e63a-497e-82da-3b629212431b", 0, "static-concurrency-stamp", "IkHoudVanPaul@example.com", true, false, null, "IKHOUDVANPAUL@EXAMPLE.COM", "IKHOUDVANPAUL@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw==", null, false, "static-security-stamp", false, "IkHoudVanPaul@example.com" },
                    { "a60c8f93-cb79-441e-8ec9-627d8a679ff3", 0, "static-concurrency-stamp", "bob.square@example.com", true, false, null, "BOB.SQUARE@EXAMPLE.COM", "BOB.SQUARE@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw==", null, false, "static-security-stamp", false, "bob.square@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2025, 1, 6), 2, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 7), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 8), 2, new TimeOnly(15, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 9), 2, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { new DateOnly(2025, 1, 10), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 11), 2, new TimeOnly(14, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 12), 2, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 13), 2, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 14), 2, new TimeOnly(17, 0, 0), new TimeOnly(13, 0, 0) },
                    { new DateOnly(2025, 1, 15), 2, new TimeOnly(15, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 16), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 17), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 18), 2, new TimeOnly(13, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 19), 2, new TimeOnly(17, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 6), 3, new TimeOnly(17, 30, 0), new TimeOnly(9, 30, 0) },
                    { new DateOnly(2025, 1, 7), 3, new TimeOnly(18, 30, 0), new TimeOnly(10, 30, 0) },
                    { new DateOnly(2025, 1, 8), 3, new TimeOnly(15, 30, 0), new TimeOnly(11, 30, 0) },
                    { new DateOnly(2025, 1, 9), 3, new TimeOnly(16, 30, 0), new TimeOnly(8, 30, 0) },
                    { new DateOnly(2025, 1, 10), 3, new TimeOnly(20, 30, 0), new TimeOnly(12, 30, 0) },
                    { new DateOnly(2025, 1, 11), 3, new TimeOnly(14, 30, 0), new TimeOnly(9, 30, 0) },
                    { new DateOnly(2025, 1, 12), 3, new TimeOnly(16, 30, 0), new TimeOnly(10, 30, 0) },
                    { new DateOnly(2025, 1, 13), 3, new TimeOnly(19, 30, 0), new TimeOnly(11, 30, 0) },
                    { new DateOnly(2025, 1, 14), 3, new TimeOnly(17, 30, 0), new TimeOnly(13, 30, 0) },
                    { new DateOnly(2025, 1, 15), 3, new TimeOnly(15, 30, 0), new TimeOnly(9, 30, 0) },
                    { new DateOnly(2025, 1, 16), 3, new TimeOnly(20, 30, 0), new TimeOnly(12, 30, 0) },
                    { new DateOnly(2025, 1, 17), 3, new TimeOnly(18, 30, 0), new TimeOnly(10, 30, 0) },
                    { new DateOnly(2025, 1, 18), 3, new TimeOnly(13, 30, 0), new TimeOnly(9, 30, 0) },
                    { new DateOnly(2025, 1, 19), 3, new TimeOnly(17, 30, 0), new TimeOnly(11, 30, 0) }
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3,
                columns: new[] { "ContractHours", "StartOfEmployment" },
                values: new object[] { 20, new DateOnly(2020, 7, 30) });

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2025, 1, 6));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2025, 1, 7));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2025, 1, 8));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2025, 1, 9));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2025, 1, 10));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2025, 1, 11));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2025, 1, 12));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2025, 1, 13));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Date",
                value: new DateOnly(2025, 1, 14));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 10,
                column: "Date",
                value: new DateOnly(2025, 1, 15));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 11,
                column: "Date",
                value: new DateOnly(2025, 1, 16));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 12,
                column: "Date",
                value: new DateOnly(2025, 1, 17));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 13,
                column: "Date",
                value: new DateOnly(2025, 1, 18));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 14,
                column: "Date",
                value: new DateOnly(2025, 1, 19));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 15,
                column: "Date",
                value: new DateOnly(2025, 1, 20));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 16,
                column: "Date",
                value: new DateOnly(2025, 1, 21));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 17,
                column: "Date",
                value: new DateOnly(2025, 1, 22));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 18,
                column: "Date",
                value: new DateOnly(2025, 1, 23));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 19,
                column: "Date",
                value: new DateOnly(2025, 1, 24));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 20,
                column: "Date",
                value: new DateOnly(2025, 1, 25));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 21,
                column: "Date",
                value: new DateOnly(2025, 1, 26));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 22,
                column: "Date",
                value: new DateOnly(2025, 1, 27));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 23,
                column: "Date",
                value: new DateOnly(2025, 1, 28));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 24,
                column: "Date",
                value: new DateOnly(2025, 1, 29));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 25,
                column: "Date",
                value: new DateOnly(2025, 1, 30));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 26,
                column: "Date",
                value: new DateOnly(2025, 1, 31));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 27,
                column: "Date",
                value: new DateOnly(2025, 2, 1));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 28,
                column: "Date",
                value: new DateOnly(2025, 2, 2));

            migrationBuilder.InsertData(
                table: "Expectations",
                columns: new[] { "Id", "Date", "ExpectedCargo", "ExpectedCustomers" },
                values: new object[,]
                {
                    { 29, new DateOnly(2025, 2, 3), 37, 820 },
                    { 30, new DateOnly(2025, 2, 4), 55, 960 },
                    { 31, new DateOnly(2025, 2, 5), 45, 840 },
                    { 32, new DateOnly(2025, 2, 6), 38, 780 },
                    { 33, new DateOnly(2025, 2, 7), 50, 900 },
                    { 34, new DateOnly(2025, 2, 8), 48, 930 },
                    { 35, new DateOnly(2025, 2, 9), 60, 1000 }
                });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeNumber", "EndDate", "Reason", "StartDate" },
                values: new object[] { 2, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bruiloft", new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Reason", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Weekendje weg", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "EmployeeNumber", "EndDate", "Reason", "StartDate", "Status" },
                values: new object[,]
                {
                    { 3, 3, new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Weekend vakantie", new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4, 3, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Geen zin om te werken dan", new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "HasBeenRead", "SentAt", "Title" },
                values: new object[] { "Er is een nieuwe verlofaanvraag om te beoordelen", true, new DateTime(2024, 12, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), "Nieuwe verlofaanvraag" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "EmployeeNumber", "HasBeenRead", "SentAt", "Title" },
                values: new object[] { "Je verlofaanvraag is beoordeeld", 2, false, new DateTime(2024, 12, 8, 15, 30, 0, 0, DateTimeKind.Unspecified), "Nieuwe verlofaanvraag status" });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "ActionUrl", "Description", "EmployeeNumber", "HasBeenRead", "SentAt", "Title" },
                values: new object[] { 3, null, "Je verlofaanvraag is beoordeeld", 3, true, new DateTime(2024, 12, 8, 15, 30, 0, 0, DateTimeKind.Unspecified), "Nieuwe verlofaanvraag status" });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2025, 1, 6));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2025, 1, 6));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2025, 1, 6));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2025, 1, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2025, 1, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2025, 1, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2025, 1, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2025, 1, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 9,
                column: "Date",
                value: new DateOnly(2025, 1, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 10,
                column: "Date",
                value: new DateOnly(2025, 1, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 11,
                column: "Date",
                value: new DateOnly(2025, 1, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 12,
                column: "Date",
                value: new DateOnly(2025, 1, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 13,
                column: "Date",
                value: new DateOnly(2025, 1, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 14,
                column: "Date",
                value: new DateOnly(2025, 1, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 15,
                column: "Date",
                value: new DateOnly(2025, 1, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 16,
                column: "Date",
                value: new DateOnly(2025, 1, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 17,
                column: "Date",
                value: new DateOnly(2025, 1, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 18,
                column: "Date",
                value: new DateOnly(2025, 1, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 19,
                column: "Date",
                value: new DateOnly(2025, 1, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 20,
                column: "Date",
                value: new DateOnly(2025, 1, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 21,
                column: "Date",
                value: new DateOnly(2025, 1, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 22,
                column: "Date",
                value: new DateOnly(2025, 1, 13));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 13), 4f, 32f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 13), 3f, 24f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 25,
                column: "Date",
                value: new DateOnly(2025, 1, 14));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 14), 3f, 24f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 27,
                column: "Date",
                value: new DateOnly(2025, 1, 14));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 28,
                column: "Date",
                value: new DateOnly(2025, 1, 15));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 15), 4f, 32f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 15), 3f, 24f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 31,
                column: "Date",
                value: new DateOnly(2025, 1, 16));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 16), 4f, 32f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 33,
                column: "Date",
                value: new DateOnly(2025, 1, 16));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 34,
                column: "Date",
                value: new DateOnly(2025, 1, 17));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 17), 4f, 32f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2025, 1, 17), 3f, 24f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 37,
                column: "Date",
                value: new DateOnly(2025, 1, 18));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 38,
                column: "Date",
                value: new DateOnly(2025, 1, 18));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 39,
                column: "Date",
                value: new DateOnly(2025, 1, 18));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 40,
                column: "Date",
                value: new DateOnly(2025, 1, 19));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 41,
                column: "Date",
                value: new DateOnly(2025, 1, 19));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 42,
                column: "Date",
                value: new DateOnly(2025, 1, 19));

            migrationBuilder.InsertData(
                table: "SchoolSchedules",
                columns: new[] { "Date", "EmployeeNumber", "DurationInHours" },
                values: new object[,]
                {
                    { new DateOnly(2025, 1, 6), 2, 6f },
                    { new DateOnly(2025, 1, 7), 2, 7f },
                    { new DateOnly(2025, 1, 8), 2, 4f },
                    { new DateOnly(2025, 1, 9), 2, 3f },
                    { new DateOnly(2025, 1, 10), 2, 5f },
                    { new DateOnly(2025, 1, 13), 2, 6.5f },
                    { new DateOnly(2025, 1, 14), 2, 7.5f },
                    { new DateOnly(2025, 1, 15), 2, 4.5f },
                    { new DateOnly(2025, 1, 16), 2, 3.5f },
                    { new DateOnly(2025, 1, 17), 2, 5.5f },
                    { new DateOnly(2025, 1, 6), 3, 6f },
                    { new DateOnly(2025, 1, 7), 3, 4f },
                    { new DateOnly(2025, 1, 8), 3, 3f },
                    { new DateOnly(2025, 1, 9), 3, 5f },
                    { new DateOnly(2025, 1, 10), 3, 7f },
                    { new DateOnly(2025, 1, 13), 3, 6.5f },
                    { new DateOnly(2025, 1, 14), 3, 4.5f },
                    { new DateOnly(2025, 1, 15), 3, 3.5f },
                    { new DateOnly(2025, 1, 16), 3, 5.5f },
                    { new DateOnly(2025, 1, 17), 3, 7.5f }
                });

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[] { 2, new DateTime(2025, 1, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Department", "End", "IsFinal", "Start" },
                values: new object[] { 0, new DateTime(2025, 1, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[,]
                {
                    { 3, 2, 2, new DateTime(2025, 1, 8, 15, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 8, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, 2, new DateTime(2025, 1, 9, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 9, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 0, 2, new DateTime(2025, 1, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 10, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, 2, new DateTime(2025, 1, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 11, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 2, 2, new DateTime(2025, 1, 12, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, 2, new DateTime(2025, 1, 13, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 13, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 0, 2, new DateTime(2025, 1, 14, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 14, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, 2, new DateTime(2025, 1, 15, 15, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 2, 2, new DateTime(2025, 1, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 16, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 0, 2, new DateTime(2025, 1, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 17, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 0, 3, new DateTime(2025, 1, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 2, 3, new DateTime(2025, 1, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 2, 3, new DateTime(2025, 1, 8, 15, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 8, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 1, 3, new DateTime(2025, 1, 9, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 9, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 2, 3, new DateTime(2025, 1, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 10, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 1, 3, new DateTime(2025, 1, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 11, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 0, 3, new DateTime(2025, 1, 12, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, 3, new DateTime(2025, 1, 13, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 13, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 2, 3, new DateTime(2025, 1, 14, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 14, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 0, 3, new DateTime(2025, 1, 15, 15, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 2, 3, new DateTime(2025, 1, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 16, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 2, 3, new DateTime(2025, 1, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 17, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 2, null, new DateTime(2025, 1, 18, 15, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 18, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 2, null, new DateTime(2025, 1, 18, 21, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 18, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 2, null, new DateTime(2025, 1, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 19, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "Name", "StartDate" },
                values: new object[] { new DateOnly(2024, 12, 24), "Kerstavond", new DateOnly(2024, 12, 24) });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Factor", "Name", "StartDate" },
                values: new object[] { new DateOnly(2025, 1, 19), 1.8f, "Weekend uitverkoop", new DateOnly(2025, 1, 18) });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "Factor", "Name", "StartDate" },
                values: new object[] { new DateOnly(2025, 1, 23), 1.5f, "Donderende donderdag korting", new DateOnly(2025, 1, 23) });

            migrationBuilder.InsertData(
                table: "UniqueDays",
                columns: new[] { "Id", "EndDate", "Factor", "Name", "StartDate" },
                values: new object[] { 4, new DateOnly(2025, 1, 27), 0.8f, "Blauwe maandag", new DateOnly(2025, 1, 27) });

            migrationBuilder.UpdateData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartDate",
                value: new DateOnly(2025, 1, 6));

            migrationBuilder.UpdateData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartDate",
                value: new DateOnly(2025, 1, 13));

            migrationBuilder.InsertData(
                table: "WeekPrognoses",
                columns: new[] { "Id", "StartDate" },
                values: new object[] { 3, new DateOnly(2025, 1, 20) });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "4b26e441-e63a-497e-82da-3b629212431b" },
                    { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "a60c8f93-cb79-441e-8ec9-627d8a679ff3" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeNumber", "ContractHours", "DateOfBirth", "EndOfEmployment", "FirstName", "HouseNumber", "LastName", "LeaveHours", "StartOfEmployment", "UserId", "Zipcode" },
                values: new object[,]
                {
                    { 4, 20, new DateOnly(2002, 6, 21), null, "Bob", "25", "van der Steen", 40, new DateOnly(2020, 6, 30), "a60c8f93-cb79-441e-8ec9-627d8a679ff3", "5622AX" },
                    { 5, 40, new DateOnly(1966, 10, 10), null, "Paul", "25", "Bakker", 40, new DateOnly(2010, 7, 30), "4b26e441-e63a-497e-82da-3b629212431b", "5622AX" }
                });

            migrationBuilder.InsertData(
                table: "Prognoses",
                columns: new[] { "Id", "Date", "Department", "NeededEmployees", "NeededHours", "WeekPrognosisId" },
                values: new object[,]
                {
                    { 43, new DateOnly(2025, 1, 20), 0, 5.25f, 42f, 3 },
                    { 44, new DateOnly(2025, 1, 20), 1, 4.25f, 34f, 3 },
                    { 45, new DateOnly(2025, 1, 20), 2, 2.75f, 22f, 3 },
                    { 46, new DateOnly(2025, 1, 21), 0, 4.25f, 34f, 3 },
                    { 47, new DateOnly(2025, 1, 21), 1, 3.25f, 26f, 3 },
                    { 48, new DateOnly(2025, 1, 21), 2, 2.25f, 18f, 3 },
                    { 49, new DateOnly(2025, 1, 22), 0, 5.75f, 46f, 3 },
                    { 50, new DateOnly(2025, 1, 22), 1, 3.5f, 28f, 3 },
                    { 51, new DateOnly(2025, 1, 22), 2, 3f, 24f, 3 },
                    { 52, new DateOnly(2025, 1, 23), 0, 6.25f, 50f, 3 },
                    { 53, new DateOnly(2025, 1, 23), 1, 4.75f, 38f, 3 },
                    { 54, new DateOnly(2025, 1, 23), 2, 2.25f, 18f, 3 },
                    { 55, new DateOnly(2025, 1, 24), 0, 5.25f, 42f, 3 },
                    { 56, new DateOnly(2025, 1, 24), 1, 3.75f, 30f, 3 },
                    { 57, new DateOnly(2025, 1, 24), 2, 1.75f, 14f, 3 },
                    { 58, new DateOnly(2025, 1, 25), 0, 4f, 32f, 3 },
                    { 59, new DateOnly(2025, 1, 25), 1, 5f, 40f, 3 },
                    { 60, new DateOnly(2025, 1, 25), 2, 3f, 24f, 3 },
                    { 61, new DateOnly(2025, 1, 26), 0, 6f, 48f, 3 },
                    { 62, new DateOnly(2025, 1, 26), 1, 4f, 32f, 3 },
                    { 63, new DateOnly(2025, 1, 26), 2, 2f, 16f, 3 }
                });

            migrationBuilder.InsertData(
                table: "ShiftTakeOvers",
                columns: new[] { "ShiftId", "EmployeeTakingOverEmployeeNumber", "Status" },
                values: new object[,]
                {
                    { 20, 2, 2 },
                    { 21, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2025, 1, 6), 4, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 7), 4, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 8), 4, new TimeOnly(15, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 9), 4, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { new DateOnly(2025, 1, 10), 4, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 11), 4, new TimeOnly(14, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 12), 4, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 13), 4, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 14), 4, new TimeOnly(17, 0, 0), new TimeOnly(13, 0, 0) },
                    { new DateOnly(2025, 1, 15), 4, new TimeOnly(15, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 16), 4, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 17), 4, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 18), 4, new TimeOnly(13, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 19), 4, new TimeOnly(17, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 6), 5, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 7), 5, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 8), 5, new TimeOnly(15, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 9), 5, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { new DateOnly(2025, 1, 10), 5, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 11), 5, new TimeOnly(14, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 12), 5, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 13), 5, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2025, 1, 14), 5, new TimeOnly(17, 0, 0), new TimeOnly(13, 0, 0) },
                    { new DateOnly(2025, 1, 15), 5, new TimeOnly(15, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 16), 5, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2025, 1, 17), 5, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2025, 1, 18), 5, new TimeOnly(13, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2025, 1, 19), 5, new TimeOnly(17, 0, 0), new TimeOnly(11, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "SchoolSchedules",
                columns: new[] { "Date", "EmployeeNumber", "DurationInHours" },
                values: new object[,]
                {
                    { new DateOnly(2025, 1, 6), 4, 6f },
                    { new DateOnly(2025, 1, 7), 4, 7f },
                    { new DateOnly(2025, 1, 8), 4, 4f },
                    { new DateOnly(2025, 1, 9), 4, 3f },
                    { new DateOnly(2025, 1, 10), 4, 5f },
                    { new DateOnly(2025, 1, 13), 4, 6.5f },
                    { new DateOnly(2025, 1, 14), 4, 7.5f },
                    { new DateOnly(2025, 1, 15), 4, 4.5f },
                    { new DateOnly(2025, 1, 16), 4, 3.5f },
                    { new DateOnly(2025, 1, 17), 4, 5.5f }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Department", "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[,]
                {
                    { 25, 0, 4, new DateTime(2025, 1, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 2, 4, new DateTime(2025, 1, 7, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 7, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1, 4, new DateTime(2025, 1, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 8, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 0, 4, new DateTime(2025, 1, 9, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 9, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 2, 4, new DateTime(2025, 1, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 10, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 0, 4, new DateTime(2025, 1, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 11, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 2, 4, new DateTime(2025, 1, 12, 17, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 12, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 1, 4, new DateTime(2025, 1, 13, 15, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 13, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 2, 4, new DateTime(2025, 1, 14, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 14, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 0, 4, new DateTime(2025, 1, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 1, 5, new DateTime(2025, 1, 6, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 6, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 2, 5, new DateTime(2025, 1, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 7, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 0, 5, new DateTime(2025, 1, 8, 16, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 2, 5, new DateTime(2025, 1, 9, 20, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 9, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 1, 5, new DateTime(2025, 1, 10, 21, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 10, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 2, 5, new DateTime(2025, 1, 11, 19, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 11, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 0, 5, new DateTime(2025, 1, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2025, 1, 12, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 2, 5, new DateTime(2025, 1, 13, 20, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 13, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 0, 5, new DateTime(2025, 1, 14, 21, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 14, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 2, 5, new DateTime(2025, 1, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 15, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 2, 5, new DateTime(2025, 1, 17, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 17, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 2, 5, new DateTime(2025, 1, 19, 19, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 1, 19, 13, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "StandardAvailabilities",
                columns: new[] { "Day", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 0, 4, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { 0, 5, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { 1, 4, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { 1, 5, new TimeOnly(15, 0, 0), new TimeOnly(7, 0, 0) },
                    { 2, 4, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { 2, 5, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { 3, 4, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { 3, 5, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { 4, 4, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { 4, 5, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { 5, 4, new TimeOnly(21, 0, 0), new TimeOnly(13, 0, 0) },
                    { 5, 5, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { 6, 4, new TimeOnly(16, 0, 0), new TimeOnly(9, 0, 0) },
                    { 6, 5, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "4b26e441-e63a-497e-82da-3b629212431b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b", "a60c8f93-cb79-441e-8ec9-627d8a679ff3" });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 11), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 12), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 18), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 19), 2 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 11), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 12), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 18), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 19), 3 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 11), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 12), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 18), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 19), 4 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 11), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 12), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 18), 5 });

            migrationBuilder.DeleteData(
                table: "Availabilities",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 19), 5 });

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 2 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 3 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 6), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 7), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 8), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 9), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 10), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 13), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 14), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 15), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 16), 4 });

            migrationBuilder.DeleteData(
                table: "SchoolSchedules",
                keyColumns: new[] { "Date", "EmployeeNumber" },
                keyValues: new object[] { new DateOnly(2025, 1, 17), 4 });

            migrationBuilder.DeleteData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ShiftTakeOvers",
                keyColumn: "ShiftId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 0, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 0, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "StandardAvailabilities",
                keyColumns: new[] { "Day", "EmployeeNumber" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b26e441-e63a-497e-82da-3b629212431b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a60c8f93-cb79-441e-8ec9-627d8a679ff3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "12544476-38da-4113-9c40-4bc508f8c0f2",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JANE", "Jane" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2667ab01-7225-451b-adbb-c99eea968d02",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "EMILY", "Emily" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2ab03136-c316-4b70-a7fc-4c9cb044a6be",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JOHN", "John" });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Date", "EmployeeNumber", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 2), 2, new TimeOnly(17, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 3), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 4), 2, new TimeOnly(15, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2024, 12, 5), 2, new TimeOnly(16, 0, 0), new TimeOnly(8, 0, 0) },
                    { new DateOnly(2024, 12, 6), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2024, 12, 7), 2, new TimeOnly(14, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 8), 2, new TimeOnly(16, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 9), 2, new TimeOnly(19, 0, 0), new TimeOnly(11, 0, 0) },
                    { new DateOnly(2024, 12, 10), 2, new TimeOnly(17, 0, 0), new TimeOnly(13, 0, 0) },
                    { new DateOnly(2024, 12, 11), 2, new TimeOnly(15, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 12), 2, new TimeOnly(20, 0, 0), new TimeOnly(12, 0, 0) },
                    { new DateOnly(2024, 12, 13), 2, new TimeOnly(18, 0, 0), new TimeOnly(10, 0, 0) },
                    { new DateOnly(2024, 12, 14), 2, new TimeOnly(13, 0, 0), new TimeOnly(9, 0, 0) },
                    { new DateOnly(2024, 12, 15), 2, new TimeOnly(17, 0, 0), new TimeOnly(11, 0, 0) }
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3,
                columns: new[] { "ContractHours", "StartOfEmployment" },
                values: new object[] { 35, new DateOnly(2019, 7, 30) });

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2024, 11, 18));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2024, 11, 19));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2024, 11, 20));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2024, 11, 21));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2024, 11, 22));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2024, 11, 23));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2024, 11, 24));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2024, 11, 25));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Date",
                value: new DateOnly(2024, 11, 26));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 10,
                column: "Date",
                value: new DateOnly(2024, 11, 27));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 11,
                column: "Date",
                value: new DateOnly(2024, 11, 28));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 12,
                column: "Date",
                value: new DateOnly(2024, 11, 29));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 13,
                column: "Date",
                value: new DateOnly(2024, 11, 30));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 14,
                column: "Date",
                value: new DateOnly(2024, 12, 1));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 15,
                column: "Date",
                value: new DateOnly(2024, 12, 2));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 16,
                column: "Date",
                value: new DateOnly(2024, 12, 3));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 17,
                column: "Date",
                value: new DateOnly(2024, 12, 4));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 18,
                column: "Date",
                value: new DateOnly(2024, 12, 5));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 19,
                column: "Date",
                value: new DateOnly(2024, 12, 6));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 20,
                column: "Date",
                value: new DateOnly(2024, 12, 7));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 21,
                column: "Date",
                value: new DateOnly(2024, 12, 8));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 22,
                column: "Date",
                value: new DateOnly(2024, 12, 9));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 23,
                column: "Date",
                value: new DateOnly(2024, 12, 10));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 24,
                column: "Date",
                value: new DateOnly(2024, 12, 11));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 25,
                column: "Date",
                value: new DateOnly(2024, 12, 12));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 26,
                column: "Date",
                value: new DateOnly(2024, 12, 13));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 27,
                column: "Date",
                value: new DateOnly(2024, 12, 14));

            migrationBuilder.UpdateData(
                table: "Expectations",
                keyColumn: "Id",
                keyValue: 28,
                column: "Date",
                value: new DateOnly(2024, 12, 15));

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeNumber", "EndDate", "Reason", "StartDate" },
                values: new object[] { 1, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family event", new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Reason", "StartDate" },
                values: new object[] { new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical appointment", new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "HasBeenRead", "SentAt", "Title" },
                values: new object[] { "Don't forget the department meeting on Dec 10.", false, new DateTime(2024, 12, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), "Meeting Reminder" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "EmployeeNumber", "HasBeenRead", "SentAt", "Title" },
                values: new object[] { "Check your holiday hours for December.", 3, true, new DateTime(2024, 12, 7, 15, 30, 0, 0, DateTimeKind.Unspecified), "Holiday Hours" });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateOnly(2024, 10, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateOnly(2024, 10, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateOnly(2024, 10, 7));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Date",
                value: new DateOnly(2024, 10, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 5,
                column: "Date",
                value: new DateOnly(2024, 10, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 6,
                column: "Date",
                value: new DateOnly(2024, 10, 8));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 7,
                column: "Date",
                value: new DateOnly(2024, 10, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 8,
                column: "Date",
                value: new DateOnly(2024, 10, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 9,
                column: "Date",
                value: new DateOnly(2024, 10, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 10,
                column: "Date",
                value: new DateOnly(2024, 10, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 11,
                column: "Date",
                value: new DateOnly(2024, 10, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 12,
                column: "Date",
                value: new DateOnly(2024, 10, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 13,
                column: "Date",
                value: new DateOnly(2024, 10, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 14,
                column: "Date",
                value: new DateOnly(2024, 10, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 15,
                column: "Date",
                value: new DateOnly(2024, 10, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 16,
                column: "Date",
                value: new DateOnly(2024, 10, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 17,
                column: "Date",
                value: new DateOnly(2024, 10, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 18,
                column: "Date",
                value: new DateOnly(2024, 10, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 19,
                column: "Date",
                value: new DateOnly(2024, 10, 13));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 20,
                column: "Date",
                value: new DateOnly(2024, 10, 13));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 21,
                column: "Date",
                value: new DateOnly(2024, 10, 13));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 22,
                column: "Date",
                value: new DateOnly(2024, 12, 9));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 9), 4.5f, 36f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 9), 2.5f, 20f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 25,
                column: "Date",
                value: new DateOnly(2024, 12, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 10), 3.5f, 28f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 27,
                column: "Date",
                value: new DateOnly(2024, 12, 10));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 28,
                column: "Date",
                value: new DateOnly(2024, 12, 11));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 11), 3f, 24f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 11), 2.5f, 20f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 31,
                column: "Date",
                value: new DateOnly(2024, 12, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 12), 4.5f, 36f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 33,
                column: "Date",
                value: new DateOnly(2024, 12, 12));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 34,
                column: "Date",
                value: new DateOnly(2024, 12, 13));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 13), 3.5f, 28f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Date", "NeededEmployees", "NeededHours" },
                values: new object[] { new DateOnly(2024, 12, 13), 1.5f, 12f });

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 37,
                column: "Date",
                value: new DateOnly(2024, 12, 14));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 38,
                column: "Date",
                value: new DateOnly(2024, 12, 14));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 39,
                column: "Date",
                value: new DateOnly(2024, 12, 14));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 40,
                column: "Date",
                value: new DateOnly(2024, 12, 15));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 41,
                column: "Date",
                value: new DateOnly(2024, 12, 15));

            migrationBuilder.UpdateData(
                table: "Prognoses",
                keyColumn: "Id",
                keyValue: 42,
                column: "Date",
                value: new DateOnly(2024, 12, 15));

            migrationBuilder.InsertData(
                table: "SchoolSchedules",
                columns: new[] { "Date", "EmployeeNumber", "DurationInHours" },
                values: new object[,]
                {
                    { new DateOnly(2024, 12, 9), 2, 3f },
                    { new DateOnly(2024, 12, 10), 2, 3f },
                    { new DateOnly(2024, 12, 11), 2, 3f },
                    { new DateOnly(2024, 12, 12), 2, 3f },
                    { new DateOnly(2024, 12, 13), 2, 3f },
                    { new DateOnly(2024, 12, 9), 3, 3f },
                    { new DateOnly(2024, 12, 10), 3, 3f },
                    { new DateOnly(2024, 12, 11), 3, 3f },
                    { new DateOnly(2024, 12, 12), 3, 3f },
                    { new DateOnly(2024, 12, 13), 3, 3f }
                });

            migrationBuilder.InsertData(
                table: "ShiftTakeOvers",
                columns: new[] { "ShiftId", "EmployeeTakingOverEmployeeNumber", "Status" },
                values: new object[] { 1, 2, 2 });

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeNumber", "End", "IsFinal", "Start" },
                values: new object[] { 1, new DateTime(2024, 12, 9, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 12, 9, 9, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Department", "End", "IsFinal", "Start" },
                values: new object[] { 1, new DateTime(2024, 12, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 12, 10, 13, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "Name", "StartDate" },
                values: new object[] { new DateOnly(2024, 11, 22), "Customer Appreciation Day", new DateOnly(2024, 11, 22) });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Factor", "Name", "StartDate" },
                values: new object[] { new DateOnly(2024, 11, 29), 1.5f, "VIP Shopping Day", new DateOnly(2024, 11, 29) });

            migrationBuilder.UpdateData(
                table: "UniqueDays",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "Factor", "Name", "StartDate" },
                values: new object[] { new DateOnly(2024, 12, 8), 1.8f, "Weekend Sale", new DateOnly(2024, 12, 7) });

            migrationBuilder.UpdateData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartDate",
                value: new DateOnly(2024, 10, 7));

            migrationBuilder.UpdateData(
                table: "WeekPrognoses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartDate",
                value: new DateOnly(2024, 12, 9));
        }
    }
}
