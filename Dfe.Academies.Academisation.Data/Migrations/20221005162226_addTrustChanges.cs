using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class addTrustChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangesToTrust",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangesToTrustExplained",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangesToTrust",
                schema: "academisation",
                table: "ApplicationJoinTrust");

            migrationBuilder.DropColumn(
                name: "ChangesToTrustExplained",
                schema: "academisation",
                table: "ApplicationJoinTrust");
        }
    }
}
