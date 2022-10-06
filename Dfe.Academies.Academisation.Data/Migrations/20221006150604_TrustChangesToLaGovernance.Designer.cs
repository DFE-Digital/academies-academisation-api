﻿// <auto-generated />
using System;
using Dfe.Academies.Academisation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    [DbContext(typeof(AcademisationContext))]
    [Migration("20221006150604_TrustChangesToLaGovernance")]
    partial class TrustChangesToLaGovernance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("academisation")
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationSchoolState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("AdditionalInformationAdded")
                        .HasColumnType("bit");

                    b.Property<string>("ApproverContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApproverContactName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CapacityAssumptions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CapacityPublishedAdmissionsNumber")
                        .HasColumnType("int");

                    b.Property<bool?>("ConfirmPaySupportGrantToSchool")
                        .HasColumnType("bit");

                    b.Property<string>("ContactChairEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactChairName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactChairTel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactHeadEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactHeadName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactHeadTel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ConversionApplicationId")
                        .HasColumnType("int");

                    b.Property<bool?>("ConversionChangeNamePlanned")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ConversionTargetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConversionTargetDateExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ConversionTargetDateSpecified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("CurrentFinancialYearCapitalCarryForward")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CurrentFinancialYearCapitalCarryForwardExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentFinancialYearCapitalCarryForwardFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrentFinancialYearCapitalCarryForwardStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CurrentFinancialYearEndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("CurrentFinancialYearRevenue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("CurrentFinancialYearRevenueStatus")
                        .HasColumnType("int");

                    b.Property<string>("CurrentFinancialYearRevenueStatusExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentFinancialYearRevenueStatusFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DeclarationBodyAgree")
                        .HasColumnType("bit");

                    b.Property<bool?>("DeclarationIAmTheChairOrHeadteacher")
                        .HasColumnType("bit");

                    b.Property<string>("DeclarationSignedByName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiocesePermissionEvidenceDocumentLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EqualitiesImpactAssessmentCompleted")
                        .HasColumnType("int");

                    b.Property<string>("EqualitiesImpactAssessmentDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FacilitiesShared")
                        .HasColumnType("bit");

                    b.Property<string>("FacilitiesSharedExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FaithSchool")
                        .HasColumnType("bit");

                    b.Property<string>("FaithSchoolDioceseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeederSchools")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FinanceOngoingInvestigations")
                        .HasColumnType("bit");

                    b.Property<string>("FinancialInvestigationsExplain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FinancialInvestigationsTrustAware")
                        .HasColumnType("bit");

                    b.Property<string>("GoverningBodyConsentEvidenceDocumentLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Grants")
                        .HasColumnType("bit");

                    b.Property<string>("GrantsAwardingBodies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HasSACREException")
                        .HasColumnType("bit");

                    b.Property<bool?>("InspectedButReportNotPublished")
                        .HasColumnType("bit");

                    b.Property<string>("InspectedButReportNotPublishedExplain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsPartOfFederation")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSupportedByFoundation")
                        .HasColumnType("bit");

                    b.Property<string>("JoinTrustReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LaClosurePlanDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LaReorganizationDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainContactOtherEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainContactOtherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainContactOtherRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainContactOtherTelephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("NextFinancialYearCapitalCarryForward")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NextFinancialYearCapitalCarryForwardExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NextFinancialYearCapitalCarryForwardFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NextFinancialYearCapitalCarryForwardStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("NextFinancialYearEndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("NextFinancialYearRevenue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("NextFinancialYearRevenueStatus")
                        .HasColumnType("int");

                    b.Property<string>("NextFinancialYearRevenueStatusExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NextFinancialYearRevenueStatusFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OngoingSafeguardingDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("OngoingSafeguardingInvestigations")
                        .HasColumnType("bit");

                    b.Property<string>("OwnerExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PartOfBuildingSchoolsForFutureProgramme")
                        .HasColumnType("bit");

                    b.Property<bool?>("PartOfLaClosurePlan")
                        .HasColumnType("bit");

                    b.Property<bool?>("PartOfLaReorganizationPlan")
                        .HasColumnType("bit");

                    b.Property<bool?>("PartOfPfiScheme")
                        .HasColumnType("bit");

                    b.Property<string>("PartOfPfiSchemeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PartOfPrioritySchoolsBuildingProgramme")
                        .HasColumnType("bit");

                    b.Property<decimal?>("PreviousFinancialYearCapitalCarryForward")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PreviousFinancialYearCapitalCarryForwardExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousFinancialYearCapitalCarryForwardFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PreviousFinancialYearCapitalCarryForwardStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PreviousFinancialYearEndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("PreviousFinancialYearRevenue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PreviousFinancialYearRevenueStatus")
                        .HasColumnType("int");

                    b.Property<string>("PreviousFinancialYearRevenueStatusExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousFinancialYearRevenueStatusFileLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectedPupilNumbersYear1")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectedPupilNumbersYear2")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectedPupilNumbersYear3")
                        .HasColumnType("int");

                    b.Property<string>("ProposedNewSchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SACREExemptionEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SchoolContributionToTrust")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("SchoolHasConsultedStakeholders")
                        .HasColumnType("bit");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolPlanToConsultStakeholders")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SupportGrantFundsPaidTo")
                        .HasColumnType("int");

                    b.Property<string>("SupportedFoundationEvidenceDocumentLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportedFoundationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Urn")
                        .HasColumnType("int");

                    b.Property<bool?>("WorksPlanned")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("WorksPlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorksPlannedExplained")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConversionApplicationId");

                    b.ToTable("ApplicationSchool", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApplicationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FormTrustId")
                        .HasColumnType("int");

                    b.Property<int?>("JoinTrustId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FormTrustId");

                    b.HasIndex("JoinTrustId");

                    b.ToTable("ConversionApplication", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ContributorState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ConversionApplicationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherRoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConversionApplicationId");

                    b.ToTable("ConversionApplicationContributor", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.FormTrustState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProposedTrustName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrustApproverName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationFormTrust", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.JoinTrustState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool?>("ChangesToLaGovernance")
                        .HasColumnType("bit");

                    b.Property<string>("ChangesToLaGovernanceExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ChangesToTrust")
                        .HasColumnType("bit");

                    b.Property<string>("ChangesToTrustExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrustName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UKPRN")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApplicationJoinTrust", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.LoanState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ApplicationSchoolId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("InterestRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Provider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Purpose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Schedule")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationSchoolId");

                    b.ToTable("ApplicationSchoolLoan", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionDeclinedReasonState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AdvisoryBoardDecisionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdvisoryBoardDecisionId");

                    b.ToTable("ConversionAdvisoryBoardDecisionDeclinedReason", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionDeferredReasonState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AdvisoryBoardDecisionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdvisoryBoardDecisionId");

                    b.ToTable("ConversionAdvisoryBoardDecisionDeferredReason", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AdvisoryBoardDecisionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApprovedConditionsDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ApprovedConditionsSet")
                        .HasColumnType("bit");

                    b.Property<int>("ConversionProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Decision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DecisionMadeBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ConversionAdvisoryBoardDecision", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ProjectAggregate.ProjectState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AcademyOrderRequired")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AcademyTypeAndRoute")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ActualPupilNumbers")
                        .HasColumnType("int");

                    b.Property<string>("AgeRange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApplicationReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApplicationReferenceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AssignedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BaselineDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<decimal?>("CapitalCarryForwardAtEndMarchCurrentYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CapitalCarryForwardAtEndMarchNextYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ClearedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Consultation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ConversionSupportGrantAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ConversionSupportGrantChangeReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiocesanConsent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiocesanTrust")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DistanceFromSchoolToTrustHeadquarters")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DistanceFromSchoolToTrustHeadquartersAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EqualitiesImpactAssessmentConsidered")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FinancialDeficit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoundationConsent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("GeneralInformationSectionComplete")
                        .HasColumnType("bit");

                    b.Property<string>("GoverningBodyResolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HeadTeacherBoardDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IfdPipelineId")
                        .HasColumnType("int");

                    b.Property<string>("KeyStage2PerformanceAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeyStage4PerformanceAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeyStage5PerformanceAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("LegalRequirementsSectionComplete")
                        .HasColumnType("bit");

                    b.Property<string>("LocalAuthority")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalAuthorityInformationTemplateComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalAuthorityInformationTemplateLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LocalAuthorityInformationTemplateReturnedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("LocalAuthorityInformationTemplateSectionComplete")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LocalAuthorityInformationTemplateSentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MemberOfParliamentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberOfParliamentParty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameOfTrust")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OpeningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PartOfPfiScheme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PercentageFreeSchoolMeals")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("PreviousHeadTeacherBoardDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PreviousHeadTeacherBoardDateQuestion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousHeadTeacherBoardLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ProjectedRevenueBalanceAtEndMarchNextYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ProposedAcademyOpeningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PublishedAdmissionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RationaleForProject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RationaleForTrust")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("RationaleSectionComplete")
                        .HasColumnType("bit");

                    b.Property<string>("RecommendationForProject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("RevenueCarryForwardAtEndMarchCurrentYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RisksAndIssues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("RisksAndIssuesSectionComplete")
                        .HasColumnType("bit");

                    b.Property<bool?>("SchoolAndTrustInformationSectionComplete")
                        .HasColumnType("bit");

                    b.Property<string>("SchoolBudgetInformationAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("SchoolBudgetInformationSectionComplete")
                        .HasColumnType("bit");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolPerformanceAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolPhase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolPupilForecastsAdditionalInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SponsorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SponsorReferenceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrustReferenceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Urn")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ViabilityIssues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("YearOneProjectedCapacity")
                        .HasColumnType("int");

                    b.Property<int?>("YearOneProjectedPupilNumbers")
                        .HasColumnType("int");

                    b.Property<int?>("YearThreeProjectedCapacity")
                        .HasColumnType("int");

                    b.Property<int?>("YearThreeProjectedPupilNumbers")
                        .HasColumnType("int");

                    b.Property<int?>("YearTwoProjectedCapacity")
                        .HasColumnType("int");

                    b.Property<int?>("YearTwoProjectedPupilNumbers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Project", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationSchoolState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", null)
                        .WithMany("Schools")
                        .HasForeignKey("ConversionApplicationId");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.FormTrustState", "FormTrust")
                        .WithMany()
                        .HasForeignKey("FormTrustId");

                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.JoinTrustState", "JoinTrust")
                        .WithMany()
                        .HasForeignKey("JoinTrustId");

                    b.Navigation("FormTrust");

                    b.Navigation("JoinTrust");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ContributorState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", null)
                        .WithMany("Contributors")
                        .HasForeignKey("ConversionApplicationId");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.LoanState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationSchoolState", null)
                        .WithMany("Loans")
                        .HasForeignKey("ApplicationSchoolId");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionDeclinedReasonState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionState", null)
                        .WithMany("DeclinedReasons")
                        .HasForeignKey("AdvisoryBoardDecisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionDeferredReasonState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionState", null)
                        .WithMany("DeferredReasons")
                        .HasForeignKey("AdvisoryBoardDecisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationSchoolState", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", b =>
                {
                    b.Navigation("Contributors");

                    b.Navigation("Schools");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecisionState", b =>
                {
                    b.Navigation("DeclinedReasons");

                    b.Navigation("DeferredReasons");
                });
#pragma warning restore 612, 618
        }
    }
}
