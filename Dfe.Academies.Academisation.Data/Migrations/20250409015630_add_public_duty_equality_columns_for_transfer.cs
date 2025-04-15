using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_public_duty_equality_columns_for_transfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicEqualityDutyImpact",
                schema: "academisation",
                table: "TransferProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicEqualityDutyReduceImpactReason",
                schema: "academisation",
                table: "TransferProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicEqualityDutySectionComplete",
                schema: "academisation",
                table: "TransferProject",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicEqualityDutyImpact",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "PublicEqualityDutyReduceImpactReason",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "PublicEqualityDutySectionComplete",
                schema: "academisation",
                table: "TransferProject");
        }
    }
}
