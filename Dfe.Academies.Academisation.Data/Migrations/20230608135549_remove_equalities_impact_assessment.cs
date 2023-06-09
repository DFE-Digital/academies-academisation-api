using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_equalities_impact_assessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EqualitiesImpactAssessmentConsidered",
                schema: "academisation",
                table: "Project");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EqualitiesImpactAssessmentConsidered",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
