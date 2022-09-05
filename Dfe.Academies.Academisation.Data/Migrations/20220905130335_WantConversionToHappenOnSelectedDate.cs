using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class WantConversionToHappenOnSelectedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WantConversionToHappenOnSelectedDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WantConversionToHappenOnSelectedDate",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
