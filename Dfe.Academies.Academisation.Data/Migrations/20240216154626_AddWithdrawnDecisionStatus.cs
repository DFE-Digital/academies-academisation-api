using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWithdrawnDecisionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvisoryBoardDecisionWithdrawnReason",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisoryBoardDecisionId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvisoryBoardDecisionWithdrawnReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvisoryBoardDecisionWithdrawnReason_ConversionAdvisoryBoardDecision_AdvisoryBoardDecisionId",
                        column: x => x.AdvisoryBoardDecisionId,
                        principalSchema: "academisation",
                        principalTable: "ConversionAdvisoryBoardDecision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvisoryBoardDecisionWithdrawnReason_AdvisoryBoardDecisionId",
                schema: "academisation",
                table: "AdvisoryBoardDecisionWithdrawnReason",
                column: "AdvisoryBoardDecisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvisoryBoardDecisionWithdrawnReason",
                schema: "academisation");
        }
    }
}
