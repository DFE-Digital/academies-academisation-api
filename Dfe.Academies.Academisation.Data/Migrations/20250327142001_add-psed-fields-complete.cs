using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpsedfieldscomplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PublicSectorEqualityDutySectionIsCompleted",
                schema: "academisation",
                table: "TransferProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicSectorEqualityDutySectionComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicSectorEqualityDutySectionIsCompleted",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "PublicSectorEqualityDutySectionComplete",
                schema: "academisation",
                table: "Project");
        }
    }
}
