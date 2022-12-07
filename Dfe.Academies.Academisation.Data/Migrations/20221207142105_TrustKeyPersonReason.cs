using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class TrustKeyPersonReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailAddress",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.RenameColumn(
                name: "TimeInRole",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                newName: "Name");

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ApplicationJoinTrust",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationFormTrustKeyPersonRole",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    TimeInRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationFormTrustKeyPersonRoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFormTrustKeyPersonRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPerson_ApplicationFormTrustKeyPersonRoleId",
                        column: x => x.ApplicationFormTrustKeyPersonRoleId,
                        principalSchema: "academisation",
                        principalTable: "ApplicationFormTrustKeyPerson",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPersonRoleId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPersonRole",
                column: "ApplicationFormTrustKeyPersonRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationFormTrustKeyPersonRole",
                schema: "academisation");

            migrationBuilder.DropColumn(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ApplicationJoinTrust");

            migrationBuilder.DropColumn(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                newName: "TimeInRole");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmailAddress",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
