﻿// <auto-generated />
using System;
using Dfe.Academies.Academisation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    [DbContext(typeof(AcademisationContext))]
    partial class AcademisationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<bool?>("FacilitiesShared")
                        .HasColumnType("bit");

                    b.Property<string>("FacilitiesSharedExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Grants")
                        .HasColumnType("bit");

                    b.Property<string>("GrantsAwardingBodies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JoinTrustReason")
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

                    b.Property<string>("OwnerExplained")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PartOfBuildingSchoolsForFutureProgramme")
                        .HasColumnType("bit");

                    b.Property<bool?>("PartOfPfiScheme")
                        .HasColumnType("bit");

                    b.Property<string>("PartOfPfiSchemeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PartOfPrioritySchoolsBuildingProgramme")
                        .HasColumnType("bit");

                    b.Property<int?>("ProjectedPupilNumbersYear1")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectedPupilNumbersYear2")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectedPupilNumbersYear3")
                        .HasColumnType("int");

                    b.Property<string>("ProposedNewSchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportGrantFundsPaidTo")
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

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

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

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ConversionLegalRequirement.LegalRequirementState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HadConsultation")
                        .HasColumnType("int");

                    b.Property<int?>("HasDioceseConsented")
                        .HasColumnType("int");

                    b.Property<int?>("HasFoundationConsented")
                        .HasColumnType("int");

                    b.Property<int?>("HaveProvidedResolution")
                        .HasColumnType("int");

                    b.Property<bool>("IsSectionComplete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LegalRequirement", "academisation");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationSchoolState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", null)
                        .WithMany("Schools")
                        .HasForeignKey("ConversionApplicationId");
                });

            modelBuilder.Entity("Dfe.Academies.Academisation.Data.ApplicationAggregate.ContributorState", b =>
                {
                    b.HasOne("Dfe.Academies.Academisation.Data.ApplicationAggregate.ApplicationState", null)
                        .WithMany("Contributors")
                        .HasForeignKey("ConversionApplicationId");
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
