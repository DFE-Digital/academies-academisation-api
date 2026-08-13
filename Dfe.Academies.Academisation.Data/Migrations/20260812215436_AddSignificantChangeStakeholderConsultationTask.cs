using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeStakeholderConsultationTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrustConsultedStakeholders",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustConsultedStakeholdersNotConsultedReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrustConsultedStakeholders",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "TrustConsultedStakeholdersNotConsultedReason",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
