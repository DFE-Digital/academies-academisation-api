using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBreakdownOfPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Details_NumberOfAlternativeProvisionPlaces",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Details_NumberOfMedicalPlaces",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Details_NumberOfPost16Places",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Details_NumberOfSENUnitPlaces",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details_NumberOfAlternativeProvisionPlaces",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_NumberOfMedicalPlaces",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_NumberOfPost16Places",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_NumberOfSENUnitPlaces",
                schema: "academisation",
                table: "Project");
        }
    }
}
