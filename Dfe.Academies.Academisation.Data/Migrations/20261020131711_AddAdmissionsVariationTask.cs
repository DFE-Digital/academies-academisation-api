using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionsVariationTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdmissionsVariationAdmissionsTeamSupportive",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdmissionsVariationLocalAuthoritySupportive",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdmissionsVariationPupilPlacePlanningSupportive",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdmissionsVariationRequired",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionsVariationSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionsVariationAdmissionsTeamSupportive",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "AdmissionsVariationLocalAuthoritySupportive",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "AdmissionsVariationPupilPlacePlanningSupportive",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "AdmissionsVariationRequired",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "AdmissionsVariationSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
