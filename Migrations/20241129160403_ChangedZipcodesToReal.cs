using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangedZipcodesToReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 1,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "1", "5583AA" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 2,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "2", "5684AS" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "1", "5683AA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 1,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "1A", "1234AB" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 2,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "2B", "5684AC" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeNumber",
                keyValue: 3,
                columns: new[] { "HouseNumber", "Zipcode" },
                values: new object[] { "3C", "5211DG" });
        }
    }
}
