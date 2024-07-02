using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class PreviousAdvisoryBoardDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousAdvisoryBoardDate",
                schema: "academisation",
                table: "TransferProject",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TransferDateSectionIsCompleted",
                schema: "academisation",
                table: "TransferProject",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousAdvisoryBoardDate",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "TransferDateSectionIsCompleted",
                schema: "academisation",
                table: "TransferProject");
        }
    }
}
