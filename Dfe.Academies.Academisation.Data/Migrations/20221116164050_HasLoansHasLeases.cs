using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class HasLoansHasLeases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasLeases",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasLoans",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasLeases",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "HasLoans",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
