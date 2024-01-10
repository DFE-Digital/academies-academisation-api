using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeOfPupilAttendingPRU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Details_PupilsAttendingGroupMedicalAndHealthNeeds",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Details_PupilsAttendingGroupPermanentlyExcluded",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Details_PupilsAttendingGroupTeenageMums",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details_PupilsAttendingGroupMedicalAndHealthNeeds",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_PupilsAttendingGroupPermanentlyExcluded",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_PupilsAttendingGroupTeenageMums",
                schema: "academisation",
                table: "Project");
        }
    }
}
