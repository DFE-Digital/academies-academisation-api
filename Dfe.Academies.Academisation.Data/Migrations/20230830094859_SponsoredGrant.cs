using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SponsoredGrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversionSupportGrantEnvironmentalImprovementGrant",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConversionSupportGrantType",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionSupportGrantEnvironmentalImprovementGrant",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ConversionSupportGrantType",
                schema: "academisation",
                table: "Project");
        }
    }
}
