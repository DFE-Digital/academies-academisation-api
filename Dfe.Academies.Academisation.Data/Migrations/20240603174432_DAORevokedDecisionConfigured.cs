using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class DAORevokedDecisionConfigured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecision_ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvisoryBoardDAORevokedReasonDetails",
                table: "AdvisoryBoardDAORevokedReasonDetails");

            migrationBuilder.DropIndex(
                name: "IX_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails");

            migrationBuilder.DropColumn(
                name: "ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails");

            migrationBuilder.RenameTable(
                name: "AdvisoryBoardDAORevokedReasonDetails",
                newName: "AdvisoryBoardDecisionDAORevokedReason",
                newSchema: "academisation");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvisoryBoardDecisionDAORevokedReason",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvisoryBoardDecisionDAORevokedReason_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason",
                column: "AdvisoryBoardDecisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisoryBoardDecisionDAORevokedReason_ConversionAdvisoryBoardDecision_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason",
                column: "AdvisoryBoardDecisionId",
                principalSchema: "academisation",
                principalTable: "ConversionAdvisoryBoardDecision",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvisoryBoardDecisionDAORevokedReason_ConversionAdvisoryBoardDecision_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvisoryBoardDecisionDAORevokedReason",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason");

            migrationBuilder.DropIndex(
                name: "IX_AdvisoryBoardDecisionDAORevokedReason_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "AdvisoryBoardDecisionDAORevokedReason");

            migrationBuilder.RenameTable(
                name: "AdvisoryBoardDecisionDAORevokedReason",
                schema: "academisation",
                newName: "AdvisoryBoardDAORevokedReasonDetails");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvisoryBoardDAORevokedReasonDetails",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                column: "ConversionAdvisoryBoardDecisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecision_ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                column: "ConversionAdvisoryBoardDecisionId",
                principalSchema: "academisation",
                principalTable: "ConversionAdvisoryBoardDecision",
                principalColumn: "Id");
        }
    }
}
