using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class ApplicationFormTrustDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedTrustName",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.AddColumn<bool>(
                name: "FormTrustGrowthPlansYesNo",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustImprovementApprovedSponsor",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustImprovementStrategy",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustImprovementSupport",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FormTrustOpeningDate",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustPlanForGrowth",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustPlansForNoGrowth",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustProposedNameOfTrust",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FormTrustReasonApprovaltoConvertasSAT",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonApprovedPerson",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonForming",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonFreedom",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonGeoAreas",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonImproveTeaching",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTrustReasonVision",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustApproverEmail",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormTrustGrowthPlansYesNo",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustImprovementApprovedSponsor",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustImprovementStrategy",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustImprovementSupport",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustOpeningDate",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustPlanForGrowth",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustPlansForNoGrowth",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustProposedNameOfTrust",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonApprovaltoConvertasSAT",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonApprovedPerson",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonForming",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonFreedom",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonGeoAreas",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonImproveTeaching",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "FormTrustReasonVision",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.DropColumn(
                name: "TrustApproverEmail",
                schema: "academisation",
                table: "ApplicationFormTrust");

            migrationBuilder.AddColumn<string>(
                name: "ProposedTrustName",
                schema: "academisation",
                table: "ApplicationFormTrust",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
