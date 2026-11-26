using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImpactOnSenProvision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImpactOnSenProvisionAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpactOnSenProvisionImprovesProvision",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpactOnSenProvisionSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImpactOnSenProvisionAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ImpactOnSenProvisionImprovesProvision",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ImpactOnSenProvisionSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
