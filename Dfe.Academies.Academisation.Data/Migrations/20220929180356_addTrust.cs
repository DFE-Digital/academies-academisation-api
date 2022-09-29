using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class addTrust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrustId",
                schema: "academisation",
                table: "ConversionApplication",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationTrust",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UKPrn = table.Column<int>(type: "int", nullable: false),
                    TrustName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrustApproverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTrust", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_TrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "TrustId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionApplication_ApplicationTrust_TrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "TrustId",
                principalSchema: "academisation",
                principalTable: "ApplicationTrust",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionApplication_ApplicationTrust_TrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropTable(
                name: "ApplicationTrust",
                schema: "academisation");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_TrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropColumn(
                name: "TrustId",
                schema: "academisation",
                table: "ConversionApplication");
        }
    }
}
