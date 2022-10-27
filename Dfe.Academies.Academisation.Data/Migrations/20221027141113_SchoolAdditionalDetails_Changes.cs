using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolAdditionalDetails_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FaithSchool",
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
                name: "IsPartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "IsSupportedByFoundation",
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

            migrationBuilder.RenameColumn(
                name: "SupportedFoundationName",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "TrustBenefitDetails");

            migrationBuilder.RenameColumn(
                name: "SupportedFoundationEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "SafeguardingDetails");

            migrationBuilder.RenameColumn(
                name: "SchoolContributionToTrust",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "ResolutionConsentFolderIdentifier");

            migrationBuilder.RenameColumn(
                name: "OngoingSafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "OfstedInspectionDetails");

            migrationBuilder.RenameColumn(
                name: "LaReorganizationDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LocalAuthorityReoganisationDetails");

            migrationBuilder.RenameColumn(
                name: "LaClosurePlanDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LocalAuthorityClosurePlanDetails");

            migrationBuilder.RenameColumn(
                name: "InspectedButReportNotPublishedExplain",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "FurtherInformation");

            migrationBuilder.RenameColumn(
                name: "GoverningBodyConsentEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "FoundationTrustOrBodyName");

            migrationBuilder.RenameColumn(
                name: "FeederSchools",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "FoundationConsentFolderIdentifier");

            migrationBuilder.RenameColumn(
                name: "FaithSchoolDioceseName",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "DioceseName");

            migrationBuilder.RenameColumn(
                name: "EqualitiesImpactAssessmentDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "DioceseFolderIdentifier");

            migrationBuilder.RenameColumn(
                name: "EqualitiesImpactAssessmentCompleted",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "SchoolSupportGrantFundsPaidTo");

            migrationBuilder.RenameColumn(
                name: "DiocesePermissionEvidenceDocumentLink",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "ApplicationJoinTrustReason");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExemptionEndDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainFeederSchools",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProtectedCharacteristics",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExemptionEndDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "MainFeederSchools",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "ProtectedCharacteristics",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.RenameColumn(
                name: "TrustBenefitDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "SupportedFoundationName");

            migrationBuilder.RenameColumn(
                name: "SchoolSupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "EqualitiesImpactAssessmentCompleted");

            migrationBuilder.RenameColumn(
                name: "SafeguardingDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "SupportedFoundationEvidenceDocumentLink");

            migrationBuilder.RenameColumn(
                name: "ResolutionConsentFolderIdentifier",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "SchoolContributionToTrust");

            migrationBuilder.RenameColumn(
                name: "OfstedInspectionDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "OngoingSafeguardingDetails");

            migrationBuilder.RenameColumn(
                name: "LocalAuthorityReoganisationDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LaReorganizationDetails");

            migrationBuilder.RenameColumn(
                name: "LocalAuthorityClosurePlanDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LaClosurePlanDetails");

            migrationBuilder.RenameColumn(
                name: "FurtherInformation",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "InspectedButReportNotPublishedExplain");

            migrationBuilder.RenameColumn(
                name: "FoundationTrustOrBodyName",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "GoverningBodyConsentEvidenceDocumentLink");

            migrationBuilder.RenameColumn(
                name: "FoundationConsentFolderIdentifier",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "FeederSchools");

            migrationBuilder.RenameColumn(
                name: "DioceseName",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "FaithSchoolDioceseName");

            migrationBuilder.RenameColumn(
                name: "DioceseFolderIdentifier",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "EqualitiesImpactAssessmentDetails");

            migrationBuilder.RenameColumn(
                name: "ApplicationJoinTrustReason",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "DiocesePermissionEvidenceDocumentLink");

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

            migrationBuilder.AddColumn<bool>(
                name: "FaithSchool",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
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
        }
    }
}
