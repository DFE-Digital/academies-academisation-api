using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveObsoleteSignificantChangeTaskColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrustConsultedReligiousBodyAnswer",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WereThereAnyObjectionsRaised",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StakeholderObjectionsDetails",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReligiousBodySupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherStakeholderEngagementSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelevantStakeholdersNotifiedOfAdmissionsChanges",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelevantStakeholdersNotifiedOfAdmissionsChangesReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReligiousBodyConsentGranted",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReligiousBodyConsentNotGrantedReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReligiousBodyRaisedObjection",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReligiousBodyRaisedObjectionReason",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE [academisation].[SignificantChangeProject]
                SET [TrustConsultedReligiousBodyAnswer] =
                    CASE [TrustConsultedReligiousBody] WHEN 1 THEN N'Yes' WHEN 0 THEN N'No' ELSE NULL END,
                    [WereThereAnyObjectionsRaised] =
                    CASE [StakeholderObjections]
                        WHEN N'No' THEN 0
                        WHEN N'YesAllObjectionsAddressed' THEN 1
                        WHEN N'YesNoFurtherInformationProvided' THEN 1
                        ELSE NULL
                    END,
                    [StakeholderObjectionsDetails] =
                    CASE WHEN [StakeholderObjections] = N'YesNoFurtherInformationProvided'
                        THEN [StakeholderObjectionsComment] ELSE NULL END;
                """);

            migrationBuilder.DropColumn(
                name: "ConsultationIncludeAdmissionVariation",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ConsultationNoAdmissionVariationReason",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "TrustConsultedReligiousBody",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "StakeholderObjections",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "StakeholderObjectionsComment",
                schema: "academisation",
                table: "SignificantChangeProject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrustConsultedReligiousBody",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StakeholderObjections",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StakeholderObjectionsComment",
                schema: "academisation",
                table: "SignificantChangeProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsultationIncludeAdmissionVariation",
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

            migrationBuilder.Sql("""
                UPDATE [academisation].[SignificantChangeProject]
                SET [TrustConsultedReligiousBody] =
                    CASE [TrustConsultedReligiousBodyAnswer] WHEN N'Yes' THEN 1 WHEN N'No' THEN 0 ELSE NULL END,
                    [StakeholderObjections] =
                    CASE
                        WHEN [WereThereAnyObjectionsRaised] = 0 THEN N'No'
                        WHEN [WereThereAnyObjectionsRaised] = 1 AND [StakeholderObjectionsDetails] IS NULL
                            THEN N'YesAllObjectionsAddressed'
                        WHEN [WereThereAnyObjectionsRaised] = 1 THEN N'YesNoFurtherInformationProvided'
                        ELSE NULL
                    END,
                    [StakeholderObjectionsComment] = [StakeholderObjectionsDetails];
                """);

            migrationBuilder.DropColumn(
                name: "OtherStakeholderEngagementSupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "RelevantStakeholdersNotifiedOfAdmissionsChanges",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "RelevantStakeholdersNotifiedOfAdmissionsChangesReason",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ReligiousBodyConsentGranted",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ReligiousBodyConsentNotGrantedReason",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ReligiousBodyRaisedObjection",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ReligiousBodyRaisedObjectionReason",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "ReligiousBodySupportingEvidence",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "StakeholderObjectionsDetails",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "TrustConsultedReligiousBodyAnswer",
                schema: "academisation",
                table: "SignificantChangeProject");

            migrationBuilder.DropColumn(
                name: "WereThereAnyObjectionsRaised",
                schema: "academisation",
                table: "SignificantChangeProject");
        }
    }
}
