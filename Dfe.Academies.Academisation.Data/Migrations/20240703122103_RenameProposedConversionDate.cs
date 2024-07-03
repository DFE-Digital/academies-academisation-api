using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProposedConversionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details_ProposedConversionDate",
                schema: "academisation",
                table: "Project",
                newName: "ProposedConversionDate");

            migrationBuilder.RenameColumn(
                name: "Details_ProjectDatesSectionComplete",
                schema: "academisation",
                table: "Project",
                newName: "ProjectDatesSectionComplete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProposedConversionDate",
                schema: "academisation",
                table: "Project",
                newName: "Details_ProposedConversionDate");

            migrationBuilder.RenameColumn(
                name: "ProjectDatesSectionComplete",
                schema: "academisation",
                table: "Project",
                newName: "Details_ProjectDatesSectionComplete");
        }
    }
}
