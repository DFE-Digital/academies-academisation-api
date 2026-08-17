using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeAdmissionsVariationConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConsultationIncludeAdmissionVariation",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsultationIncludeAdmissionVariationNotApplicable",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultationNoAdmissionVariationReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultationIncludeAdmissionVariation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ConsultationIncludeAdmissionVariationNotApplicable",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ConsultationNoAdmissionVariationReason",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
