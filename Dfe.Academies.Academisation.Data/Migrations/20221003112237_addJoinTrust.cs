using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class addJoinTrust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationFormTrust",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposedTrustName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrustApproverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFormTrust", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationJoinTrust",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UKPrn = table.Column<int>(type: "int", nullable: false),
                    TrustName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationJoinTrust", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "FormTrustId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "JoinTrustId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionApplication_ApplicationFormTrust_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "FormTrustId",
                principalSchema: "academisation",
                principalTable: "ApplicationFormTrust",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionApplication_ApplicationJoinTrust_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "JoinTrustId",
                principalSchema: "academisation",
                principalTable: "ApplicationJoinTrust",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionApplication_ApplicationFormTrust_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversionApplication_ApplicationJoinTrust_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropTable(
                name: "ApplicationFormTrust",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "ApplicationJoinTrust",
                schema: "academisation");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropColumn(
                name: "FormTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropColumn(
                name: "JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication");
        }
    }
}
