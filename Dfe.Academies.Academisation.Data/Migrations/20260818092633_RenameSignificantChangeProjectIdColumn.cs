using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSignificantChangeProjectIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdvisoryBoardDecisionDetails_SignificantChangeProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                newName: "SignificantChangeProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignificantChangeProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                newName: "AdvisoryBoardDecisionDetails_SignificantChangeProjectId");
        }
    }
}
