using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeClearance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionsGroupClearance",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustRegulationClearance",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionsGroupClearance",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "TrustRegulationClearance",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
