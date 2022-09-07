using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolConversionTargetDateSpecified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConversionTargetDateSpecified",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionTargetDateSpecified",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
