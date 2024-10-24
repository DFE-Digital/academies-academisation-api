using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorreadOnlyflagtoadate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.RenameColumn(
                name: "ProjectSentToCompleteDate",
                schema: "academisation",
                table: "Project",
                newName: "ReadOnlyDate");

            migrationBuilder.RenameColumn(
                name: "IsReadOnly",
                schema: "academisation",
                table: "Project",
                newName: "ProjectSentToComplete");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadOnlyDate",
                schema: "academisation",
                table: "TransferProject",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadOnlyDate",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.RenameColumn(
                name: "ReadOnlyDate",
                schema: "academisation",
                table: "Project",
                newName: "ProjectSentToCompleteDate");

            migrationBuilder.RenameColumn(
                name: "ProjectSentToComplete",
                schema: "academisation",
                table: "Project",
                newName: "IsReadOnly");

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                schema: "academisation",
                table: "TransferProject",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
