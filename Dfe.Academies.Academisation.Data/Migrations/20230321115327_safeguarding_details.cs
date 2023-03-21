using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class safeguarding_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.AddColumn<bool>(
                name: "Safeguarding",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Safeguarding",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.AddColumn<string>(
                name: "SafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
