using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class uniqueIndexToExpectations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expectations_Date",
                table: "Expectations",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expectations_Date",
                table: "Expectations");
        }
    }
}
