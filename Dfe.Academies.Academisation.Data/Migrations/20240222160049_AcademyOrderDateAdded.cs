using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AcademyOrderDateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademyOrderRequired",
                schema: "academisation",
                table: "Project");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcademyOrderDate",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademyOrderDate",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision");

            migrationBuilder.AddColumn<string>(
                name: "AcademyOrderRequired",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
