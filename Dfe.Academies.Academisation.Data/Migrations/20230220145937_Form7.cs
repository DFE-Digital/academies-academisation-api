using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Form7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Form7Received",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Form7ReceivedDate",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Form7Received",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Form7ReceivedDate",
                schema: "academisation",
                table: "Project");
        }
    }
}
