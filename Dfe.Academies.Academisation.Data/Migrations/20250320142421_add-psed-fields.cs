using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpsedfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HowLikelyImpactProtectedCharacteristics",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatWillBeDoneToReduceImpact",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HowLikelyImpactProtectedCharacteristics",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "WhatWillBeDoneToReduceImpact",
                schema: "academisation",
                table: "Project");
        }
    }
}
