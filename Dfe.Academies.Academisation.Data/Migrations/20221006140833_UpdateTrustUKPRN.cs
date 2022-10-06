using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class UpdateTrustUKPRN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UKPrn",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                newName: "UKPRN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UKPRN",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                newName: "UKPrn");
        }
    }
}
