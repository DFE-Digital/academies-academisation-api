using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolAdditionalInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdditionalInformationAdded",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiocesePermissionEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EqualitiesImpactAssessmentCompleted",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EqualitiesImpactAssessmentDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FaithSchool",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaithSchoolDioceseName",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeederSchools",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoverningBodyConsentEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSACREException",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InspectedButReportNotPublished",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectedButReportNotPublishedExplain",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupportedByFoundation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaClosurePlanDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaReorganizationDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OngoingSafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OngoingSafeguardingInvestigations",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartOfLaClosurePlan",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartOfLaReorganizationPlan",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SACREExemptionEndDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolContributionToTrust",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportedFoundationEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportedFoundationName",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "AdditionalInformationAdded",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "DiocesePermissionEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "EqualitiesImpactAssessmentCompleted",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "EqualitiesImpactAssessmentDetails",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "FaithSchool",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "FaithSchoolDioceseName",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "FeederSchools",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "GoverningBodyConsentEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "HasSACREException",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "InspectedButReportNotPublished",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "InspectedButReportNotPublishedExplain",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "IsPartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "IsSupportedByFoundation",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "LaClosurePlanDetails",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "LaReorganizationDetails",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "OngoingSafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "OngoingSafeguardingInvestigations",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfLaClosurePlan",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfLaReorganizationPlan",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SACREExemptionEndDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SchoolContributionToTrust",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SupportedFoundationEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SupportedFoundationName",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.AlterColumn<string>(
                name: "SupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
