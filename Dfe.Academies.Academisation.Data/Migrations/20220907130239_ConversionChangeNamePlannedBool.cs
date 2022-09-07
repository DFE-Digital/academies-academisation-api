using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class ConversionChangeNamePlannedBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConversionChangeNamePlanned",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionChangeNamePlanned",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
