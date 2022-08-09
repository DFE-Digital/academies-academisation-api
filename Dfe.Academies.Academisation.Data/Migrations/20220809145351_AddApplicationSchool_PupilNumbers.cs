using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddApplicationSchool_PupilNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectedPupilNumbersYear1",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectedPupilNumbersYear2",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectedPupilNumbersYear3",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedNewSchoolName",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolCapacityAssumptions",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolCapacityPublishedAdmissionsNumber",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectedPupilNumbersYear1",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "ProjectedPupilNumbersYear2",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "ProjectedPupilNumbersYear3",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "ProposedNewSchoolName",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SchoolCapacityAssumptions",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SchoolCapacityPublishedAdmissionsNumber",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
