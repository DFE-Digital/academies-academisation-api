using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Add_legal_requirements_to_project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Consultation",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiocesanConsent",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoundationConsent",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoverningBodyResolution",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LegalRequirementsSectionComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consultation",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DiocesanConsent",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "FoundationConsent",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "GoverningBodyResolution",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "LegalRequirementsSectionComplete",
                schema: "academisation",
                table: "Project");
        }
    }
}
