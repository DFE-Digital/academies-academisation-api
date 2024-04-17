using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class schooldatabudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfCurrentFinancialYear",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfNextFinancialYear",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOfCurrentFinancialYear",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EndOfNextFinancialYear",
                schema: "academisation",
                table: "Project");
        }
    }
}
