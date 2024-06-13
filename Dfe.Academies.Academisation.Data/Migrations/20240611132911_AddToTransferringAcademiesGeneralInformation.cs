using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddToTransferringAcademiesGeneralInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistanceFromAcademyToTrustHq",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceFromAcademyToTrustHqDetails",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialDeficit",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MPNameAndParty",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublishedAdmissionNumber",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViabilityIssues",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceFromAcademyToTrustHq",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "DistanceFromAcademyToTrustHqDetails",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "FinancialDeficit",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "MPNameAndParty",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "PublishedAdmissionNumber",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "ViabilityIssues",
                schema: "academisation",
                table: "TransferringAcademy");
        }
    }
}
