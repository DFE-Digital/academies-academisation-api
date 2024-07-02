using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransferDatesIsCompletedDuplicateRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferDateSectionIsCompleted",
                schema: "academisation",
                table: "TransferProject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TransferDateSectionIsCompleted",
                schema: "academisation",
                table: "TransferProject",
                type: "bit",
                nullable: true);
        }
    }
}
