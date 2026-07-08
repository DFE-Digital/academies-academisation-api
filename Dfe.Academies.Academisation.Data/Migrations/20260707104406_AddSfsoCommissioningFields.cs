using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSfsoCommissioningFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SfsoCommissioningOverview",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SfsoCommissioningRequestedDate",
                schema: "academisation",
                table: "Project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SfsoCommissioningSectionComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SfsoCommissioningOverview",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "SfsoCommissioningRequestedDate",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "SfsoCommissioningSectionComplete",
                schema: "academisation",
                table: "Project");
        }
    }
}
