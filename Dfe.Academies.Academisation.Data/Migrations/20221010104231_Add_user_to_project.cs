using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Add_user_to_project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserEmailAddress",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserFullName",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserId",
                schema: "academisation",
                table: "Project",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserEmailAddress",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AssignedUserFullName",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                schema: "academisation",
                table: "Project");
        }
    }
}
