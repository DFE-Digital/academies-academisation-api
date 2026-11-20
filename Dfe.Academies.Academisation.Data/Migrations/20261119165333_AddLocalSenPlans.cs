using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalSenPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalSenPlansAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalSenPlansConflict",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalSenPlansSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalSenPlansAdditionalInformation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "LocalSenPlansConflict",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "LocalSenPlansSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
