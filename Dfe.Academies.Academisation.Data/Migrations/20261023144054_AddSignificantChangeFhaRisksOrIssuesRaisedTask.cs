using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeFhaRisksOrIssuesRaisedTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FhaRisksOrIssuesRaised",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FhaRisksOrIssuesRaisedAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FhaRisksOrIssuesRaisedSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FhaRisksOrIssuesRaised",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "FhaRisksOrIssuesRaisedAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "FhaRisksOrIssuesRaisedSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
