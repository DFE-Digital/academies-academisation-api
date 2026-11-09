using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEvidenceLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupportingEvidenceLink",
                schema: "academisation",
                table: "SignificantChangeProject",
                newName: "LocalAuthoritySupportingEvidenceLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalAuthoritySupportingEvidenceLink",
                schema: "academisation",
                table: "SignificantChangeProject",
                newName: "SupportingEvidenceLink");
        }
    }
}
