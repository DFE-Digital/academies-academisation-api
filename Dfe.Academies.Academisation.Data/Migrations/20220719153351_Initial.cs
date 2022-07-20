using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academisation");

            migrationBuilder.CreateTable(
                name: "ConversionApplication",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionApplication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversionApplicationContributor",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    OtherRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversionApplicationId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionApplicationContributor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversionApplicationContributor_ConversionApplication_ConversionApplicationId",
                        column: x => x.ConversionApplicationId,
                        principalSchema: "academisation",
                        principalTable: "ConversionApplication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplicationContributor_ConversionApplicationId",
                schema: "academisation",
                table: "ConversionApplicationContributor",
                column: "ConversionApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionApplicationContributor",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "ConversionApplication",
                schema: "academisation");
        }
    }
}
