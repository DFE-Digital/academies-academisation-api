using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeReligiousBodyConsultationTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrustConsultedReligiousBody",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustConsultedReligiousBodyNotConsultedReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrustConsultedReligiousBody",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "TrustConsultedReligiousBodyNotConsultedReason",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
