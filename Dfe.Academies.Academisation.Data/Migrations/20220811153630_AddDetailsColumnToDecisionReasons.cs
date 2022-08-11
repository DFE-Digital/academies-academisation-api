using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddDetailsColumnToDecisionReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclinedOtherReason",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision");

            migrationBuilder.DropColumn(
                name: "DeferredOtherReason",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeferredReason",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeclinedReason",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeferredReason");

            migrationBuilder.DropColumn(
                name: "Details",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeclinedReason");

            migrationBuilder.AddColumn<string>(
                name: "DeclinedOtherReason",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeferredOtherReason",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
