using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeDecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReadOnlyDate",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvisoryBoardDecisionDetails_SignificantChangeProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadOnlyDate",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "AdvisoryBoardDecisionDetails_SignificantChangeProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision");
        }
    }
}
