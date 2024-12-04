using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BumboApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckConstraintsTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Availability_StartTime_EndTime",
                table: "Availabilities");

            migrationBuilder.AddCheckConstraint(
                name: "CK_StandardAvailability_StartTime_EndTime",
                table: "StandardAvailabilities",
                sql: "[StartTime] <= [EndTime]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Availability_StartTime_EndTime",
                table: "Availabilities",
                sql: "[StartTime] <= [EndTime]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_StandardAvailability_StartTime_EndTime",
                table: "StandardAvailabilities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Availability_StartTime_EndTime",
                table: "Availabilities");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Availability_StartTime_EndTime",
                table: "Availabilities",
                sql: "[StartTime] < [EndTime]");
        }
    }
}
