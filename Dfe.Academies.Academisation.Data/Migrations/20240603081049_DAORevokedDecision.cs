using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class DAORevokedDecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvisoryBoardDAORevokedReasonDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvisoryBoardDecisionId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConversionAdvisoryBoardDecisionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvisoryBoardDAORevokedReasonDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecision_ConversionAdvisoryBoardDecisionId",
                        column: x => x.ConversionAdvisoryBoardDecisionId,
                        principalSchema: "academisation",
                        principalTable: "ConversionAdvisoryBoardDecision",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvisoryBoardDAORevokedReasonDetails_ConversionAdvisoryBoardDecisionId",
                table: "AdvisoryBoardDAORevokedReasonDetails",
                column: "ConversionAdvisoryBoardDecisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvisoryBoardDAORevokedReasonDetails");
        }
    }
}
