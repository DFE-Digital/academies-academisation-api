using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddApplicationSchool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSchool",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    ConversionApplicationId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSchool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationSchool_ConversionApplication_ConversionApplicationId",
                        column: x => x.ConversionApplicationId,
                        principalSchema: "academisation",
                        principalTable: "ConversionApplication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSchool_ConversionApplicationId",
                schema: "academisation",
                table: "ApplicationSchool",
                column: "ConversionApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSchool",
                schema: "academisation");
        }
    }
}
