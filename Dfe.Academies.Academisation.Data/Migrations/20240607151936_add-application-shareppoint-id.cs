using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addapplicationshareppointid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SharePointId",
                schema: "academisation",
                table: "Project",
                newName: "SchoolSharePointId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationSharePointId",
                schema: "academisation",
                table: "Project",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationSharePointId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "SchoolSharePointId",
                schema: "academisation",
                table: "Project",
                newName: "SharePointId");
        }
    }
}
