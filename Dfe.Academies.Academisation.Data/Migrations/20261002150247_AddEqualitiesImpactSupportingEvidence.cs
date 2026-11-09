using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEqualitiesImpactSupportingEvidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.AddColumn<string>(
		        name: "EqualitiesImpactSupportingEvidence",
		        schema: "academisation",
		        table: "SignificantChangeProject",
		        type: "nvarchar(max)",
		        nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropColumn(
		        name: "EqualitiesImpactSupportingEvidence",
		        schema: "academisation",
		        table: "SignificantChangeProject");
        }
    }
}
