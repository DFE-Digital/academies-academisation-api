using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class ApplicationPreSupportGrant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmPaySupportGrantToSchool",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPaySupportGrantToSchool",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
