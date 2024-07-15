using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectDatesSectionComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ProjectDatesSectionComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectDatesSectionComplete",
                schema: "academisation",
                table: "Project");
        }
    }
}
