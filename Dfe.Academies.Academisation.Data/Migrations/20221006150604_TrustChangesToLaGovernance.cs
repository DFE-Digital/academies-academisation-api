using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class TrustChangesToLaGovernance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangesToLaGovernance",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangesToLaGovernanceExplained",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangesToLaGovernance",
                schema: "academisation",
                table: "ApplicationJoinTrust");

            migrationBuilder.DropColumn(
                name: "ChangesToLaGovernanceExplained",
                schema: "academisation",
                table: "ApplicationJoinTrust");
        }
    }
}
