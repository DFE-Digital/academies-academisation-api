using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddApplicationSchool_LandAndBuildings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FacilitiesShared",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacilitiesSharedExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Grants",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrantsAwardingBodies",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartOfBuildingSchoolsForFutureProgramme",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartOfPfiScheme",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartOfPfiSchemeType",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartOfPrioritySchoolsBuildingProgramme",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WorksPlanned",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorksPlannedDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorksPlannedExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilitiesShared",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "FacilitiesSharedExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "Grants",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "GrantsAwardingBodies",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "OwnerExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfBuildingSchoolsForFutureProgramme",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfPfiScheme",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfPfiSchemeType",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfPrioritySchoolsBuildingProgramme",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "WorksPlanned",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "WorksPlannedDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "WorksPlannedExplained",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
