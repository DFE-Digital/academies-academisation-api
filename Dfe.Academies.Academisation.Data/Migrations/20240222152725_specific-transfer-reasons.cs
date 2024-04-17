using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class specifictransferreasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademyOrderRequired",
                schema: "academisation",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "SpecificReasonsForTransfer",
                schema: "academisation",
                table: "TransferProject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecificReasonsForTransfer",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.AddColumn<string>(
                name: "AcademyOrderRequired",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
