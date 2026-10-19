using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeHqtfConsiderationTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HqtfConsiderationAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HqtfConsiderationAnswer",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HqtfConsiderationSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HqtfConsiderationAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "HqtfConsiderationAnswer",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "HqtfConsiderationSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
