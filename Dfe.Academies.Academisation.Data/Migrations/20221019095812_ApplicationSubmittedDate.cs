using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class ApplicationSubmittedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationSubmittedDate",
                schema: "academisation",
                table: "ConversionApplication",
                type: "datetime2",
                nullable: true);        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationSubmittedDate",
                schema: "academisation",
                table: "ConversionApplication");
            
        }
    }
}
