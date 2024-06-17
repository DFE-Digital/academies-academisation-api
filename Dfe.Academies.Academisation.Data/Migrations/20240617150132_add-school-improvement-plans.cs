using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class addschoolimprovementplans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolImprovementPlans",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ArrangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrangedByOther = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvidedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedEndDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedEndDateOther = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfidenceLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolImprovementPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolImprovementPlans_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "academisation",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImprovementPlans_ProjectId",
                schema: "academisation",
                table: "SchoolImprovementPlans",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolImprovementPlans",
                schema: "academisation");
        }
    }
}
