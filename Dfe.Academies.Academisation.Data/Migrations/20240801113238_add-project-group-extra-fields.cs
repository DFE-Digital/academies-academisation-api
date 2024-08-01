using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addprojectgroupextrafields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrustName",
                schema: "academisation",
                table: "ProjectGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrustUkprn",
                schema: "academisation",
                table: "ProjectGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrustName",
                schema: "academisation",
                table: "ProjectGroups");

            migrationBuilder.DropColumn(
                name: "TrustUkprn",
                schema: "academisation",
                table: "ProjectGroups");
        }
    }
}
