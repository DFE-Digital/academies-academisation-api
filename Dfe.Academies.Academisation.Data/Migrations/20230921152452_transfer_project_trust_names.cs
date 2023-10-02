using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class transfer_project_trust_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IncomingTrustName",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingTrustName",
                schema: "academisation",
                table: "TransferProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomingTrustName",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "OutgoingTrustName",
                schema: "academisation",
                table: "TransferProject");
        }
    }
}
