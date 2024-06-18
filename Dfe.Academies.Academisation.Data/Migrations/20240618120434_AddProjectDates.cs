using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Details_AdvisoryBoardDate",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Details_PreviousAdvisoryBoard",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details_AdvisoryBoardDate",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_PreviousAdvisoryBoard",
                schema: "academisation",
                table: "Project");
        }
    }
}
