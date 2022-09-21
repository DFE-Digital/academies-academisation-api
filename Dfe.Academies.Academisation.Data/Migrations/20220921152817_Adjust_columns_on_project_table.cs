using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Adjust_columns_on_project_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Laestab",
                schema: "academisation",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "Upin",
                schema: "academisation",
                table: "Project",
                newName: "SchoolType");

            migrationBuilder.RenameColumn(
                name: "UkPrn",
                schema: "academisation",
                table: "Project",
                newName: "SchoolPhase");

            migrationBuilder.RenameColumn(
                name: "NewAcademyUrn",
                schema: "academisation",
                table: "Project",
                newName: "DiocesanTrust");

            migrationBuilder.RenameColumn(
                name: "CurrentYearPupilNumbers",
                schema: "academisation",
                table: "Project",
                newName: "IfdPipelineId");

            migrationBuilder.RenameColumn(
                name: "CurrentYearCapacity",
                schema: "academisation",
                table: "Project",
                newName: "Capacity");

            migrationBuilder.AddColumn<int>(
                name: "ActualPupilNumbers",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgeRange",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageFreeSchoolMeals",
                schema: "academisation",
                table: "Project",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust",
                schema: "academisation",
                table: "Project",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualPupilNumbers",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AgeRange",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PercentageFreeSchoolMeals",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust",
                schema: "academisation",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "SchoolType",
                schema: "academisation",
                table: "Project",
                newName: "Upin");

            migrationBuilder.RenameColumn(
                name: "SchoolPhase",
                schema: "academisation",
                table: "Project",
                newName: "UkPrn");

            migrationBuilder.RenameColumn(
                name: "IfdPipelineId",
                schema: "academisation",
                table: "Project",
                newName: "CurrentYearPupilNumbers");

            migrationBuilder.RenameColumn(
                name: "DiocesanTrust",
                schema: "academisation",
                table: "Project",
                newName: "NewAcademyUrn");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                schema: "academisation",
                table: "Project",
                newName: "CurrentYearCapacity");

            migrationBuilder.AddColumn<int>(
                name: "Laestab",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
