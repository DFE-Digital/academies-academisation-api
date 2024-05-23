using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtransferringacademyregionlocalauthority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalAuthority",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalAuthority",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "academisation",
                table: "TransferringAcademy");
        }
    }
}
