using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorcompletetransmissionlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.AddColumn<bool>(
                name: "ProjectSentToComplete",
                schema: "academisation",
                table: "TransferringAcademy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProjectSentToComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "CompleteTransmissionLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransferringAcademyId",
                schema: "academisation",
                table: "CompleteTransmissionLog",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectSentToComplete",
                schema: "academisation",
                table: "TransferringAcademy");

            migrationBuilder.DropColumn(
                name: "ProjectSentToComplete",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "CompleteTransmissionLog");

            migrationBuilder.DropColumn(
                name: "TransferringAcademyId",
                schema: "academisation",
                table: "CompleteTransmissionLog");

            migrationBuilder.AddColumn<Guid>(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "TransferProject",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompleteProjectId",
                schema: "academisation",
                table: "Project",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
