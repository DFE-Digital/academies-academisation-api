using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class OpeningDateHistoryReasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OpeningDateHistories",
                newName: "OpeningDateHistories",
                newSchema: "academisation");

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "academisation",
                table: "OpeningDateHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonsChanged",
                schema: "academisation",
                table: "OpeningDateHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "academisation",
                table: "OpeningDateHistories");

            migrationBuilder.DropColumn(
                name: "ReasonsChanged",
                schema: "academisation",
                table: "OpeningDateHistories");

            migrationBuilder.RenameTable(
                name: "OpeningDateHistories",
                schema: "academisation",
                newName: "OpeningDateHistories");
        }
    }
}
