using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class FormAMAtAssignedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserEmailAddress",
                schema: "academisation",
                table: "FormAMatProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserFullName",
                schema: "academisation",
                table: "FormAMatProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserId",
                schema: "academisation",
                table: "FormAMatProject",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserEmailAddress",
                schema: "academisation",
                table: "FormAMatProject");

            migrationBuilder.DropColumn(
                name: "AssignedUserFullName",
                schema: "academisation",
                table: "FormAMatProject");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                schema: "academisation",
                table: "FormAMatProject");
        }
    }
}
