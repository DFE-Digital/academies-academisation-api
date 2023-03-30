using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class addannexbformdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnnexBFormReceived",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnnexBFormUrl",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnexBFormReceived",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AnnexBFormUrl",
                schema: "academisation",
                table: "Project");
        }
    }
}
