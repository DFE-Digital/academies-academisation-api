using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddCreateConversionProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversionAdvisoryBoardDecision",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversionProjectId = table.Column<int>(type: "int", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedConditionsSet = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedConditionsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeclinedOtherReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeferredOtherReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvisoryBoardDecisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionMadeBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionAdvisoryBoardDecision", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversionAdvisoryBoardDecisionDeclinedReason",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisoryBoardDecisionId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionAdvisoryBoardDecisionDeclinedReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversionAdvisoryBoardDecisionDeclinedReason_ConversionAdvisoryBoardDecision_AdvisoryBoardDecisionId",
                        column: x => x.AdvisoryBoardDecisionId,
                        principalSchema: "academisation",
                        principalTable: "ConversionAdvisoryBoardDecision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversionAdvisoryBoardDecisionDeferredReason",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisoryBoardDecisionId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionAdvisoryBoardDecisionDeferredReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversionAdvisoryBoardDecisionDeferredReason_ConversionAdvisoryBoardDecision_AdvisoryBoardDecisionId",
                        column: x => x.AdvisoryBoardDecisionId,
                        principalSchema: "academisation",
                        principalTable: "ConversionAdvisoryBoardDecision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionAdvisoryBoardDecisionDeclinedReason_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeclinedReason",
                column: "AdvisoryBoardDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionAdvisoryBoardDecisionDeferredReason_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecisionDeferredReason",
                column: "AdvisoryBoardDecisionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionAdvisoryBoardDecisionDeclinedReason",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "ConversionAdvisoryBoardDecisionDeferredReason",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "ConversionAdvisoryBoardDecision",
                schema: "academisation");
        }
    }
}
