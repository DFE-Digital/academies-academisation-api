using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEqualitiesLikelyDetailsAndAddSomeImpactDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EqualitiesImpactIdentifiedMitigation",
                schema: "academisation",
                table: "SignificantChangeProject",
                newName: "EqualitiesLikelyDetails");

            migrationBuilder.AddColumn<string>(
                name: "EqualitiesSomeImpactDetails",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EqualitiesSomeImpactDetails",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.RenameColumn(
                name: "EqualitiesLikelyDetails",
                schema: "academisation",
                table: "SignificantChangeProject",
                newName: "EqualitiesImpactIdentifiedMitigation");
        }
    }
}
