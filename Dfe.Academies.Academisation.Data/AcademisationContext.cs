using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext, IUnitOfWork
{
	const string DEFAULT_SCHEMA = "academisation";
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options)
	{

	}

	public DbSet<Application> Applications { get; set; } = null!; // done
	public DbSet<Contributor> Contributors { get; set; } = null!; // done
	public DbSet<School> Schools { get; set; } = null!; // done
	public DbSet<Loan> SchoolLoans { get; set; } = null!;  // done
	public DbSet<Lease> SchoolLeases { get; set; } = null!; // done

	public DbSet<JoinTrust> JoinTrusts { get; set; } = null!; // done
	public DbSet<FormTrust> FormTrusts { get; set; } = null!; // done

	public DbSet<Project> Projects { get; set; } = null!;
	public DbSet<ProjectNote> ProjectNotes { get; set; } = null!;
	public DbSet<ConversionAdvisoryBoardDecisionState> ConversionAdvisoryBoardDecisions { get; set; } = null!;

	public DbSet<TransferProject> TransferProjects { get; set; } = null!;

	public override int SaveChanges()
	{
		SetModifiedAndCreatedDates();
		return base.SaveChanges();
	}

	public EntityEntry<T> ReplaceTracked<T>(T baseEntity) where T : BaseEntity
	{
		var entity = ChangeTracker
			.Entries<T>()
			.SingleOrDefault(s => s.Entity.Id == baseEntity.Id);

		if (entity is null) throw new InvalidOperationException("An entity matching this Id is not being tracked");

		var childCollections = entity.Collections
			.Select(collection => collection.CurrentValue)
			.Select(childCollection => childCollection);

		foreach (var children in childCollections)
		{
			if (children is null) continue;
			foreach (var child in children) Remove(child);
		}

		entity.State = EntityState.Detached;

		return Update(baseEntity);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
	{
		SetModifiedAndCreatedDates();
		return base.SaveChangesAsync(cancellationToken);
	}

	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		//This will be used to dispatch domain events in mediator before committing changes with SaveChangesAsync
		/*
		 * await _mediator.DispatchDomainEventsAsync(this);
		 */
		//SetModifiedAndCreatedDates();
		await base.SaveChangesAsync(cancellationToken);
		return true;
	}

	private void SetModifiedAndCreatedDates()
	{
		var timestamp = DateTime.UtcNow;

		var entities = ChangeTracker.Entries<BaseEntity>().ToList();

		foreach (var entity in entities.Where(e => e.State == EntityState.Added))
		{
			entity.Entity.CreatedOn = timestamp;
			entity.Entity.LastModifiedOn = timestamp;
		}

		foreach (var entity in entities.Where(e => e.State == EntityState.Modified))
		{
			entity.Entity.LastModifiedOn = timestamp;
		}

		// for new domain object mapped directly to the database
		var domainEntities = ChangeTracker.Entries<Entity>().ToList();

		foreach (var entity in domainEntities.Where(e => e.State == EntityState.Added))
		{
			entity.Entity.CreatedOn = timestamp;
			entity.Entity.LastModifiedOn = timestamp;
		}

		foreach (var entity in domainEntities.Where(e => e.State == EntityState.Modified))
		{
			entity.Entity.LastModifiedOn = timestamp;
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Application>(ConfigureApplication);
		modelBuilder.Entity<School>(ConfigureSchool);
		modelBuilder.Entity<Contributor>(ConfigureContributor);
		modelBuilder.Entity<FormTrust>(ConfigureFormTrust);
		modelBuilder.Entity<JoinTrust>(ConfigureJoinTrust);
		modelBuilder.Entity<Loan>(ConfigureLoan);
		modelBuilder.Entity<Lease>(ConfigureLease);
		modelBuilder.Entity<TrustKeyPerson>(ConfigureTrustKeyPerson);
		modelBuilder.Entity<TrustKeyPersonRole>(ConfigureTrustKeyPersonRole);

		modelBuilder.Entity<Project>(ConfigureProject);
		modelBuilder.Entity<ProjectNote>(ConfigureProjectNotes);

		modelBuilder.Entity<ConversionAdvisoryBoardDecisionState>(ConfigureConversionAdvisoryBoardDecision);
		modelBuilder.Entity<ConversionAdvisoryBoardDecisionDeferredReasonState>(ConfigureConversionAdvisoryBoardDecisionDeferredReason);
		modelBuilder.Entity<ConversionAdvisoryBoardDecisionDeclinedReasonState>(ConfigureConversionAdvisoryBoardDecisionDeclinedReason);

		modelBuilder.Entity<FormAMatProject>(ConfigureFormAMatProject);

		// Replicatiing functionality to generate urn, this will have to be ofset as part of the migration when we go live
		modelBuilder.Entity<TransferProject>(ConfigureTransferProject);
		modelBuilder.Entity<IntendedTransferBenefit>(ConfigureTransferProjectIntendedTransferBenefit);
		modelBuilder.Entity<TransferringAcademy>(ConfigureTransferringAcademy);

		base.OnModelCreating(modelBuilder);
	}
	private static void ConfigureFormAMatProject(EntityTypeBuilder<FormAMatProject> formAMatProjectConfiguration)
	{
		formAMatProjectConfiguration.ToTable("FormAMatProject", DEFAULT_SCHEMA);
		formAMatProjectConfiguration.HasKey(x => x.Id);
	}

	private void ConfigureTransferringAcademy(EntityTypeBuilder<TransferringAcademy> transferringAcademy)
	{
		transferringAcademy.ToTable("TransferringAcademy", DEFAULT_SCHEMA);
		transferringAcademy.HasKey(x => x.Id);
	}

	private void ConfigureTransferProjectIntendedTransferBenefit(EntityTypeBuilder<IntendedTransferBenefit> transferProjectIntendedTransferBenefit)
	{
		transferProjectIntendedTransferBenefit.ToTable("IntendedTransferBenefit", DEFAULT_SCHEMA);
		transferProjectIntendedTransferBenefit.HasKey(x => x.Id);
	}

	void ConfigureTransferProject(EntityTypeBuilder<TransferProject> transferProject)
	{
		transferProject.ToTable("TransferProject", DEFAULT_SCHEMA);
		transferProject.HasKey(x => x.Id);
		transferProject.Property(p => p.Id).UseIdentityColumn(10003000, 1);

		transferProject
		.HasMany(a => a.IntendedTransferBenefits)
		.WithOne()
		.HasForeignKey(x => x.TransferProjectId)
		.IsRequired();

		transferProject
		.HasMany(a => a.TransferringAcademies)
		.WithOne()
		.HasForeignKey(x => x.TransferProjectId)
		.IsRequired();

		var intendedTransferBenefitsNavigation = transferProject.Metadata.FindNavigation(nameof(TransferProject.IntendedTransferBenefits));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the TransferProjectIntendedTransferBenefits collection property through its field
		intendedTransferBenefitsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		var transferringAcademiesNavigation = transferProject.Metadata.FindNavigation(nameof(TransferProject.TransferringAcademies));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the TransferProjectIntendedTransferBenefits collection property through its field
		transferringAcademiesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	private static void ConfigureProject(EntityTypeBuilder<Project> projectConfiguration)
	{
		projectConfiguration.ToTable("Project", DEFAULT_SCHEMA);
		projectConfiguration.HasKey(x => x.Id);
		projectConfiguration.Property(d => d.CreatedOn).HasColumnName("CreatedOn");
		projectConfiguration.Property(d => d.FormAMatProjectId).HasColumnName("FormAMatProjectId");
		projectConfiguration.OwnsOne(x => x.Details, pd =>
		{
			pd.Property(d => d.ViabilityIssues).HasColumnName("ViabilityIssues");
			pd.Property(d => d.PreviousHeadTeacherBoardDateQuestion).HasColumnName("PreviousHeadTeacherBoardDateQuestion");
			pd.Property(d => d.PartOfPfiScheme).HasColumnName("PartOfPfiScheme");
			pd.Property(d => d.LocalAuthorityInformationTemplateSectionComplete).HasColumnName("LocalAuthorityInformationTemplateSectionComplete");

			pd.Property(d => d.AgeRange).HasColumnName("AgeRange");
			pd.Property(d => d.RisksAndIssuesSectionComplete).HasColumnName("RisksAndIssuesSectionComplete");
			pd.Property(d => d.RisksAndIssues).HasColumnName("RisksAndIssues");
			pd.Property(d => d.RationaleSectionComplete).HasColumnName("RationaleSectionComplete");
			pd.Property(d => d.RationaleForTrust).HasColumnName("RationaleForTrust");
			pd.Property(d => d.RationaleForProject).HasColumnName("RationaleForProject");
			pd.Property(d => d.SchoolPerformanceAdditionalInformation).HasColumnName("SchoolPerformanceAdditionalInformation");
			pd.Property(d => d.SchoolOverviewSectionComplete).HasColumnName("SchoolOverviewSectionComplete");
			pd.Property(d => d.MemberOfParliamentNameAndParty).HasColumnName("MemberOfParliamentNameAndParty");
			pd.Property(d => d.DistanceFromSchoolToTrustHeadquartersAdditionalInformation).HasColumnName("DistanceFromSchoolToTrustHeadquartersAdditionalInformation");
			pd.Property(d => d.DistanceFromSchoolToTrustHeadquarters).HasColumnName("DistanceFromSchoolToTrustHeadquarters");
			pd.Property(d => d.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust).HasColumnName("PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust");
			pd.Property(d => d.DiocesanTrust).HasColumnName("DiocesanTrust");
			pd.Property(d => d.FinancialDeficit).HasColumnName("FinancialDeficit");
			pd.Property(d => d.PfiSchemeDetails).HasColumnName("PfiSchemeDetails");
			pd.Property(d => d.PercentageFreeSchoolMeals).HasColumnName("PercentageFreeSchoolMeals");
			pd.Property(d => d.PublishedAdmissionNumber).HasColumnName("PublishedAdmissionNumber");
			pd.Property(d => d.Capacity).HasColumnName("Capacity");
			pd.Property(d => d.GoverningBodyResolution).HasColumnName("GoverningBodyResolution").HasConversion<string>(); ;
			pd.Property(d => d.Consultation).HasColumnName("Consultation").HasConversion<string>();
			pd.Property(d => d.DiocesanConsent).HasColumnName("DiocesanConsent").HasConversion<string>(); ;
			pd.Property(d => d.FoundationConsent).HasColumnName("FoundationConsent").HasConversion<string>(); ;
			pd.OwnsOne(a => a.AssignedUser, a =>
			{
				a.Property(p => p.Id).HasColumnName("AssignedUserId");
				a.Property(p => p.EmailAddress).HasColumnName("AssignedUserEmailAddress");
				a.Property(p => p.FullName).HasColumnName("AssignedUserFullName");
			});
			pd.Property(d => d.KeyStage5PerformanceAdditionalInformation).HasColumnName("KeyStage5PerformanceAdditionalInformation");
			pd.Property(d => d.KeyStage4PerformanceAdditionalInformation).HasColumnName("KeyStage4PerformanceAdditionalInformation");
			pd.Property(d => d.KeyStage2PerformanceAdditionalInformation).HasColumnName("KeyStage2PerformanceAdditionalInformation");
			pd.Property(d => d.SchoolPupilForecastsAdditionalInformation).HasColumnName("SchoolPupilForecastsAdditionalInformation");
			pd.Property(d => d.YearThreeProjectedPupilNumbers).HasColumnName("YearThreeProjectedPupilNumbers");
			pd.Property(d => d.YearThreeProjectedCapacity).HasColumnName("YearThreeProjectedCapacity");
			pd.Property(d => d.YearTwoProjectedPupilNumbers).HasColumnName("YearTwoProjectedPupilNumbers");
			pd.Property(d => d.YearTwoProjectedCapacity).HasColumnName("YearTwoProjectedCapacity");
			pd.Property(d => d.ActualPupilNumbers).HasColumnName("ActualPupilNumbers");
			pd.Property(d => d.YearOneProjectedPupilNumbers).HasColumnName("YearOneProjectedPupilNumbers");
			pd.Property(d => d.SchoolBudgetInformationSectionComplete).HasColumnName("SchoolBudgetInformationSectionComplete");
			pd.Property(d => d.SchoolBudgetInformationAdditionalInformation).HasColumnName("SchoolBudgetInformationAdditionalInformation");
			pd.Property(d => d.CapitalCarryForwardAtEndMarchNextYear).HasColumnName("CapitalCarryForwardAtEndMarchNextYear");
			pd.Property(d => d.CapitalCarryForwardAtEndMarchCurrentYear).HasColumnName("CapitalCarryForwardAtEndMarchCurrentYear");
			pd.Property(d => d.ProjectedRevenueBalanceAtEndMarchNextYear).HasColumnName("ProjectedRevenueBalanceAtEndMarchNextYear");
			pd.Property(d => d.RevenueCarryForwardAtEndMarchCurrentYear).HasColumnName("RevenueCarryForwardAtEndMarchCurrentYear");
			pd.Property(d => d.EndOfNextFinancialYear).HasColumnName("EndOfNextFinancialYear");
			pd.Property(d => d.EndOfCurrentFinancialYear).HasColumnName("EndOfCurrentFinancialYear");
			pd.Property(d => d.LegalRequirementsSectionComplete).HasColumnName("LegalRequirementsSectionComplete");
			pd.Property(d => d.YearOneProjectedCapacity).HasColumnName("YearOneProjectedCapacity");
			pd.Property(d => d.SchoolType).HasColumnName("SchoolType");
			pd.Property(d => d.SchoolPhase).HasColumnName("SchoolPhase");
			pd.Property(d => d.RecommendationForProject).HasColumnName("RecommendationForProject");
			pd.Property(d => d.LocalAuthorityInformationTemplateLink).HasColumnName("LocalAuthorityInformationTemplateLink");
			pd.Property(d => d.LocalAuthorityInformationTemplateComments).HasColumnName("LocalAuthorityInformationTemplateComments");
			pd.Property(d => d.Form7Received).HasColumnName("Form7Received");
			pd.Property(d => d.Form7ReceivedDate).HasColumnName("Form7ReceivedDate");
			pd.Property(d => d.LocalAuthorityInformationTemplateReturnedDate).HasColumnName("LocalAuthorityInformationTemplateReturnedDate");
			pd.Property(d => d.LocalAuthorityInformationTemplateSentDate).HasColumnName("LocalAuthorityInformationTemplateSentDate");
			pd.Property(d => d.Author).HasColumnName("Author");
			pd.Property(d => d.BaselineDate).HasColumnName("BaselineDate");
			pd.Property(d => d.AssignedDate).HasColumnName("AssignedDate");
			pd.Property(d => d.ApplicationReceivedDate).HasColumnName("ApplicationReceivedDate");
			pd.Property(d => d.ProjectStatus).HasColumnName("ProjectStatus");
			pd.Property(d => d.ApplicationReferenceNumber).HasColumnName("ApplicationReferenceNumber");
			pd.Property(d => d.LocalAuthority).HasColumnName("LocalAuthority");
			pd.Property(d => d.SchoolName).HasColumnName("SchoolName");
			pd.Property(d => d.IfdPipelineId).HasColumnName("IfdPipelineId");
			pd.Property(d => d.Urn).HasColumnName("Urn");
			pd.Property(d => d.HeadTeacherBoardDate).HasColumnName("HeadTeacherBoardDate");
			pd.Property(d => d.Version).HasColumnName("Version");
			pd.Property(d => d.AcademyOrderRequired).HasColumnName("AcademyOrderRequired");
			pd.Property(d => d.Region).HasColumnName("Region");
			pd.Property(d => d.ConversionSupportGrantAmountChanged).HasColumnName("ConversionSupportGrantAmountChanged");
			pd.Property(d => d.ConversionSupportGrantEnvironmentalImprovementGrant).HasColumnName("ConversionSupportGrantEnvironmentalImprovementGrant");
			pd.Property(d => d.ConversionSupportGrantType).HasColumnName("ConversionSupportGrantType");
			pd.Property(d => d.ConversionSupportGrantChangeReason).HasColumnName("ConversionSupportGrantChangeReason");
			pd.Property(d => d.ConversionSupportGrantAmount).HasColumnName("ConversionSupportGrantAmount");
			pd.Property(d => d.SchoolAndTrustInformationSectionComplete).HasColumnName("SchoolAndTrustInformationSectionComplete");
			pd.Property(d => d.ProposedAcademyOpeningDate).HasColumnName("ProposedAcademyOpeningDate");
			pd.Property(d => d.ClearedBy).HasColumnName("ClearedBy");
			pd.Property(d => d.AcademyTypeAndRoute).HasColumnName("AcademyTypeAndRoute");
			pd.Property(d => d.SponsorReferenceNumber).HasColumnName("SponsorReferenceNumber");
			pd.Property(d => d.NameOfTrust).HasColumnName("NameOfTrust");
			pd.Property(d => d.TrustReferenceNumber).HasColumnName("TrustReferenceNumber");
			pd.Property(d => d.PreviousHeadTeacherBoardLink).HasColumnName("PreviousHeadTeacherBoardLink");
			pd.Property(d => d.PreviousHeadTeacherBoardDate).HasColumnName("PreviousHeadTeacherBoardDate");
			pd.Property(d => d.AnnexBFormUrl).HasColumnName("AnnexBFormUrl");
			pd.Property(d => d.AnnexBFormReceived).HasColumnName("AnnexBFormReceived");
			pd.Property(d => d.DaoPackSentDate).HasColumnName("DaoPackSentDate");
			pd.Property(d => d.SponsorName).HasColumnName("SponsorName");

			pd.Property(d => d.ExternalApplicationFormSaved).HasColumnName("ExternalApplicationFormSaved");
			pd.Property(d => d.ExternalApplicationFormUrl).HasColumnName("ExternalApplicationFormUrl");

		});

		projectConfiguration
			.HasMany(a => a.Notes)
			.WithOne()
			.HasForeignKey("ProjectId")
			.IsRequired();

		var notesNavigation = projectConfiguration.Metadata.FindNavigation(nameof(Project.Notes));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		notesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

	}

	private static void ConfigureProjectNotes(EntityTypeBuilder<ProjectNote> projectNoteConfiguration)
	{
		projectNoteConfiguration.ToTable("ProjectNotes", DEFAULT_SCHEMA);
		projectNoteConfiguration.HasKey(a => a.Id);
		//projectNoteConfiguration.Property<int?>("ProjectId").IsRequired(false);
	}

	private static void ConfigureConversionAdvisoryBoardDecision(EntityTypeBuilder<ConversionAdvisoryBoardDecisionState> ConversionAdvisoryBoardDecisionConfiguration)
	{
		ConversionAdvisoryBoardDecisionConfiguration.ToTable("ConversionAdvisoryBoardDecision", DEFAULT_SCHEMA);
		ConversionAdvisoryBoardDecisionConfiguration
			.Property(e => e.DecisionMadeBy)
			.HasConversion<string>();

		ConversionAdvisoryBoardDecisionConfiguration
			.Property(e => e.Decision)
			.HasConversion<string>();
	}

	private static void ConfigureConversionAdvisoryBoardDecisionDeferredReason(EntityTypeBuilder<ConversionAdvisoryBoardDecisionDeferredReasonState> ConversionAdvisoryBoardDecisionDeferredReasonConfiguration)
	{
		ConversionAdvisoryBoardDecisionDeferredReasonConfiguration.ToTable("ConversionAdvisoryBoardDecisionDeferredReason", DEFAULT_SCHEMA);
		ConversionAdvisoryBoardDecisionDeferredReasonConfiguration
			.Property(e => e.Reason)
			.HasConversion<string>();
	}

	private static void ConfigureConversionAdvisoryBoardDecisionDeclinedReason(EntityTypeBuilder<ConversionAdvisoryBoardDecisionDeclinedReasonState> ConversionAdvisoryBoardDecisionDeclinedReasonConfiguration)
	{
		ConversionAdvisoryBoardDecisionDeclinedReasonConfiguration.ToTable("ConversionAdvisoryBoardDecisionDeclinedReason", DEFAULT_SCHEMA);
		ConversionAdvisoryBoardDecisionDeclinedReasonConfiguration
			.Property(e => e.Reason)
			.HasConversion<string>();
	}

	/// <summary>
	/// New mapping for refactoring
	/// </summary>
	/// <param name="applicationConfiguration"></param>

	void ConfigureApplication(EntityTypeBuilder<Application> applicationConfiguration)
	{
		applicationConfiguration.ToTable("ConversionApplication", DEFAULT_SCHEMA);
		applicationConfiguration
		.Property(e => e.ApplicationType)
			.HasConversion<string>();

		applicationConfiguration
		.Property(e => e.ApplicationStatus)
			.HasConversion<string>();

		applicationConfiguration
			.Property(p => p.ApplicationReference)
			.HasComputedColumnSql("'A2B_' + CAST([Id] AS NVARCHAR(255))", stored: true);
		applicationConfiguration.Ignore(x => x.ApplicationId);


		applicationConfiguration.HasKey(a => a.Id);

		applicationConfiguration.Property<int?>("JoinTrustId")
			.IsRequired(false);
		applicationConfiguration
			.HasOne(x => x.JoinTrust)
			.WithOne()
			.HasForeignKey<Application>("JoinTrustId")
			.IsRequired(false);

		applicationConfiguration.Property<int?>("FormTrustId")
			.IsRequired(false);
		applicationConfiguration
			.HasOne(x => x.FormTrust)
			.WithOne()
			.HasForeignKey<Application>("FormTrustId")
			.IsRequired(false);

		applicationConfiguration
			.HasMany(a => a.Schools)
			.WithOne()
			.HasForeignKey("ConversionApplicationId")
			.IsRequired();

		applicationConfiguration
			.HasQueryFilter(p => p.DeletedAt == null);



		var schoolNavigation = applicationConfiguration.Metadata.FindNavigation(nameof(Application.Schools));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		schoolNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		applicationConfiguration
			.HasMany(a => a.Contributors)
			.WithOne()
			.HasForeignKey("ConversionApplicationId")
			.IsRequired();

		var contributorNavigation = applicationConfiguration.Metadata.FindNavigation(nameof(Application.Contributors));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		contributorNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureSchool(EntityTypeBuilder<School> schoolConfiguration)
	{
		schoolConfiguration.ToTable("ApplicationSchool", DEFAULT_SCHEMA);
		schoolConfiguration.HasKey(a => a.Id);
		schoolConfiguration.Property<int?>("ConversionApplicationId")
			.IsRequired(false);

		schoolConfiguration.OwnsOne(x => x.Details, sd =>
		{
			sd.Property(d => d.ApproverContactEmail).HasColumnName("ApproverContactEmail");
			sd.Property(d => d.ApproverContactName).HasColumnName("ApproverContactName");
			sd.Property(d => d.CapacityAssumptions).HasColumnName("CapacityAssumptions");
			sd.Property(d => d.CapacityPublishedAdmissionsNumber).HasColumnName("CapacityPublishedAdmissionsNumber");
			sd.Property(d => d.ConfirmPaySupportGrantToSchool).HasColumnName("ConfirmPaySupportGrantToSchool");
			sd.Property(d => d.ContactChairEmail).HasColumnName("ContactChairEmail");
			sd.Property(d => d.ContactChairName).HasColumnName("ContactChairName");
			sd.Property(d => d.ContactHeadEmail).HasColumnName("ContactHeadEmail");
			sd.Property(d => d.ContactHeadName).HasColumnName("ContactHeadName");
			sd.Property(d => d.ContactRole).HasColumnName("ContactRole");
			sd.Property(d => d.ConversionChangeNamePlanned).HasColumnName("ConversionChangeNamePlanned");
			sd.Property(d => d.ConversionTargetDate).HasColumnName("ConversionTargetDate");
			sd.Property(d => d.ConversionTargetDateExplained).HasColumnName("ConversionTargetDateExplained");
			sd.Property(d => d.ConversionTargetDateSpecified).HasColumnName("ConversionTargetDateSpecified");
			sd.Property(d => d.DeclarationBodyAgree).HasColumnName("DeclarationBodyAgree");
			sd.Property(d => d.DeclarationIAmTheChairOrHeadteacher).HasColumnName("DeclarationIAmTheChairOrHeadteacher");
			sd.Property(d => d.DeclarationSignedByName).HasColumnName("DeclarationSignedByName");
			sd.Property(d => d.ApplicationJoinTrustReason).HasColumnName("JoinTrustReason");
			sd.Property(d => d.Urn).HasColumnName("Urn");
			sd.Property(d => d.SchoolName).HasColumnName("SchoolName");
			sd.Property(d => d.SchoolSupportGrantFundsPaidTo).HasColumnName("SupportGrantFundsPaidTo");
			sd.Property(d => d.SchoolPlanToConsultStakeholders).HasColumnName("SchoolPlanToConsultStakeholders");

			sd.Property(d => d.FinanceOngoingInvestigations).HasColumnName("FinanceOngoingInvestigations");
			sd.Property(d => d.FinancialInvestigationsExplain).HasColumnName("FinancialInvestigationsExplain");
			sd.Property(d => d.FinancialInvestigationsTrustAware).HasColumnName("FinancialInvestigationsTrustAware");

			sd.Property(d => d.MainContactOtherEmail).HasColumnName("MainContactOtherEmail");
			sd.Property(d => d.MainContactOtherName).HasColumnName("MainContactOtherName");
			sd.Property(d => d.MainContactOtherRole).HasColumnName("MainContactOtherRole");

			sd.Property(d => d.ProjectedPupilNumbersYear1).HasColumnName("ProjectedPupilNumbersYear1");
			sd.Property(d => d.ProjectedPupilNumbersYear2).HasColumnName("ProjectedPupilNumbersYear2");
			sd.Property(d => d.ProjectedPupilNumbersYear3).HasColumnName("ProjectedPupilNumbersYear3");

			sd.Property(d => d.ProposedNewSchoolName).HasColumnName("ProposedNewSchoolName");
			sd.Property(d => d.SchoolConversionReasonsForJoining).HasColumnName("SchoolConversionReasonsForJoining");
			sd.Property(d => d.SchoolHasConsultedStakeholders).HasColumnName("SchoolHasConsultedStakeholders");

			sd.OwnsOne(d => d.PreviousFinancialYear, pfy =>
			{
				pfy.Property(pfyd => pfyd.CapitalCarryForward)
					.HasColumnName("PreviousFinancialYearCapitalCarryForward");
				pfy.Property(pfyd => pfyd.CapitalCarryForwardExplained)
					.HasColumnName("PreviousFinancialYearCapitalCarryForwardExplained");
				pfy.Property(pfyd => pfyd.CapitalCarryForwardFileLink)
					.HasColumnName("PreviousFinancialYearCapitalCarryForwardFileLink");
				pfy.Property(pfyd => pfyd.CapitalCarryForwardStatus)
					.HasColumnName("PreviousFinancialYearCapitalCarryForwardStatus");
				pfy.Property(pfyd => pfyd.FinancialYearEndDate)
					.HasColumnName("PreviousFinancialYearEndDate");
				pfy.Property(pfyd => pfyd.Revenue)
					.HasColumnName("PreviousFinancialYearRevenue");
				pfy.Property(pfyd => pfyd.RevenueStatus)
					.HasColumnName("PreviousFinancialYearRevenueStatus");
				pfy.Property(pfyd => pfyd.RevenueStatusExplained)
					.HasColumnName("PreviousFinancialYearRevenueStatusExplained");
				pfy.Property(pfyd => pfyd.RevenueStatusFileLink)
					.HasColumnName("PreviousFinancialYearRevenueStatusFileLink");
			});
			sd.OwnsOne(d => d.CurrentFinancialYear, cfy =>
			{
				cfy.Property(cfyd => cfyd.CapitalCarryForward)
					.HasColumnName("CurrentFinancialYearCapitalCarryForward");
				cfy.Property(cfyd => cfyd.CapitalCarryForwardExplained)
					.HasColumnName("CurrentFinancialYearCapitalCarryForwardExplained");
				cfy.Property(cfyd => cfyd.CapitalCarryForwardFileLink)
					.HasColumnName("CurrentFinancialYearCapitalCarryForwardFileLink");
				cfy.Property(cfyd => cfyd.CapitalCarryForwardStatus)
					.HasColumnName("CurrentFinancialYearCapitalCarryForwardStatus");
				cfy.Property(cfyd => cfyd.FinancialYearEndDate)
					.HasColumnName("CurrentFinancialYearEndDate");
				cfy.Property(cfyd => cfyd.Revenue)
					.HasColumnName("CurrentFinancialYearRevenue");
				cfy.Property(cfyd => cfyd.RevenueStatus)
					.HasColumnName("CurrentFinancialYearRevenueStatus");
				cfy.Property(cfyd => cfyd.RevenueStatusExplained)
					.HasColumnName("CurrentFinancialYearRevenueStatusExplained");
				cfy.Property(cfyd => cfyd.RevenueStatusFileLink)
					.HasColumnName("CurrentFinancialYearRevenueStatusFileLink");
			});
			sd.OwnsOne(d => d.NextFinancialYear, nfy =>
			{
				nfy.Property(nfyd => nfyd.CapitalCarryForward)
					.HasColumnName("NextFinancialYearCapitalCarryForward");
				nfy.Property(nfyd => nfyd.CapitalCarryForwardExplained)
					.HasColumnName("NextFinancialYearCapitalCarryForwardExplained");
				nfy.Property(nfyd => nfyd.CapitalCarryForwardFileLink)
					.HasColumnName("NextFinancialYearCapitalCarryForwardFileLink");
				nfy.Property(nfyd => nfyd.CapitalCarryForwardStatus)
					.HasColumnName("NextFinancialYearCapitalCarryForwardStatus");
				nfy.Property(nfyd => nfyd.FinancialYearEndDate)
					.HasColumnName("NextFinancialYearEndDate");
				nfy.Property(nfyd => nfyd.Revenue)
					.HasColumnName("NextFinancialYearRevenue");
				nfy.Property(nfyd => nfyd.RevenueStatus)
					.HasColumnName("NextFinancialYearRevenueStatus");
				nfy.Property(nfyd => nfyd.RevenueStatusExplained)
					.HasColumnName("NextFinancialYearRevenueStatusExplained");
				nfy.Property(nfyd => nfyd.RevenueStatusFileLink)
					.HasColumnName("NextFinancialYearRevenueStatusFileLink");
			});
			sd.OwnsOne(d => d.LandAndBuildings, lb =>
			{
				lb.Property(lbd => lbd.FacilitiesShared).HasColumnName("FacilitiesShared");
				lb.Property(lbd => lbd.FacilitiesSharedExplained).HasColumnName("FacilitiesSharedExplained");
				lb.Property(lbd => lbd.Grants).HasColumnName("Grants");
				lb.Property(lbd => lbd.GrantsAwardingBodies).HasColumnName("GrantsAwardingBodies");
				lb.Property(lbd => lbd.OwnerExplained).HasColumnName("OwnerExplained");
				lb.Property(lbd => lbd.PartOfBuildingSchoolsForFutureProgramme).HasColumnName("PartOfBuildingSchoolsForFutureProgramme");
				lb.Property(lbd => lbd.PartOfPfiScheme).HasColumnName("PartOfPfiScheme");
				lb.Property(lbd => lbd.PartOfPfiSchemeType).HasColumnName("PartOfPfiSchemeType");
				lb.Property(lbd => lbd.PartOfPrioritySchoolsBuildingProgramme).HasColumnName("PartOfPrioritySchoolsBuildingProgramme");
				lb.Property(lbd => lbd.WorksPlanned).HasColumnName("WorksPlanned");
				lb.Property(lbd => lbd.WorksPlannedDate).HasColumnName("WorksPlannedDate");
				lb.Property(lbd => lbd.WorksPlannedExplained).HasColumnName("WorksPlannedExplained");
				lb.Property(lbd => lbd.WorksPlannedExplained).HasColumnName("WorksPlannedExplained");
			});
		});
		schoolConfiguration
			.HasMany(a => a.Loans)
			.WithOne()
			.HasForeignKey("ApplicationSchoolId")
			.IsRequired();

		var loanNavigation = schoolConfiguration.Metadata.FindNavigation(nameof(School.Loans));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		loanNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		schoolConfiguration
			.HasMany(a => a.Leases)
			.WithOne()
			.HasForeignKey("ApplicationSchoolId")
			.IsRequired();

		var leaseNavigation = schoolConfiguration.Metadata.FindNavigation(nameof(School.Leases));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		leaseNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureContributor(EntityTypeBuilder<Contributor> contributorConfiguration)
	{
		contributorConfiguration.ToTable("ConversionApplicationContributor", DEFAULT_SCHEMA);
		contributorConfiguration.HasKey(a => a.Id);
		contributorConfiguration.Property<int?>("ConversionApplicationId")
			.IsRequired(false);

		contributorConfiguration.OwnsOne(x => x.Details, c =>
		{
			c.Property(cd => cd.FirstName).HasColumnName("FirstName");
			c.Property(cd => cd.LastName).HasColumnName("LastName");
			c.Property(cd => cd.EmailAddress).HasColumnName("EmailAddress");
			c.Property(cd => cd.Role).HasColumnName("Role");
			c.Property(cd => cd.OtherRoleName).HasColumnName("OtherRoleName");
		});

	}

	void ConfigureFormTrust(EntityTypeBuilder<FormTrust> formTrustConfiguration)
	{
		formTrustConfiguration.ToTable("ApplicationFormTrust", DEFAULT_SCHEMA);
		formTrustConfiguration.HasKey(a => a.Id);

		formTrustConfiguration.OwnsOne(x => x.TrustDetails, t =>
		{
			t.Property(td => td.FormTrustGrowthPlansYesNo).HasColumnName("FormTrustGrowthPlansYesNo");
			t.Property(td => td.FormTrustImprovementApprovedSponsor).HasColumnName("FormTrustImprovementApprovedSponsor");
			t.Property(td => td.FormTrustImprovementStrategy).HasColumnName("FormTrustImprovementStrategy");
			t.Property(td => td.FormTrustImprovementSupport).HasColumnName("FormTrustImprovementSupport");
			t.Property(td => td.FormTrustOpeningDate).HasColumnName("FormTrustOpeningDate");
			t.Property(td => td.FormTrustPlanForGrowth).HasColumnName("FormTrustPlanForGrowth");
			t.Property(td => td.FormTrustPlansForNoGrowth).HasColumnName("FormTrustPlansForNoGrowth");
			t.Property(td => td.FormTrustProposedNameOfTrust).HasColumnName("FormTrustProposedNameOfTrust");
			t.Property(td => td.FormTrustReasonApprovaltoConvertasSAT).HasColumnName("FormTrustReasonApprovaltoConvertasSAT");
			t.Property(td => td.FormTrustReasonApprovedPerson).HasColumnName("FormTrustReasonApprovedPerson");
			t.Property(td => td.FormTrustReasonForming).HasColumnName("FormTrustReasonForming");
			t.Property(td => td.FormTrustReasonFreedom).HasColumnName("FormTrustReasonFreedom");
			t.Property(td => td.FormTrustReasonGeoAreas).HasColumnName("FormTrustReasonGeoAreas");
			t.Property(td => td.FormTrustReasonImproveTeaching).HasColumnName("FormTrustReasonImproveTeaching");
			t.Property(td => td.FormTrustReasonVision).HasColumnName("FormTrustReasonVision");
			t.Property(td => td.TrustApproverEmail).HasColumnName("TrustApproverEmail");
			t.Property(td => td.TrustApproverName).HasColumnName("TrustApproverName");
		});

		formTrustConfiguration
			.HasMany(a => a.KeyPeople)
			.WithOne()
			.HasForeignKey("ApplicationFormTrustId")
			.IsRequired();

		var navigation = formTrustConfiguration.Metadata.FindNavigation(nameof(FormTrust.KeyPeople));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureJoinTrust(EntityTypeBuilder<JoinTrust> joinTrustConfiguration)
	{
		joinTrustConfiguration.ToTable("ApplicationJoinTrust", DEFAULT_SCHEMA);
		joinTrustConfiguration.HasKey(a => a.Id);
	}

	void ConfigureLoan(EntityTypeBuilder<Loan> loanConfiguration)
	{
		loanConfiguration.ToTable("ApplicationSchoolLoan", DEFAULT_SCHEMA);
		loanConfiguration.HasKey(a => a.Id);
		loanConfiguration.Property<int?>("ApplicationSchoolId").IsRequired(false);
	}

	void ConfigureLease(EntityTypeBuilder<Lease> leaseConfiguration)
	{
		leaseConfiguration.ToTable("ApplicationSchoolLease", DEFAULT_SCHEMA);
		leaseConfiguration.HasKey(a => a.Id);
		leaseConfiguration.Property<int?>("ApplicationSchoolId").IsRequired(false);
	}

	void ConfigureTrustKeyPerson(EntityTypeBuilder<TrustKeyPerson> trustKeyPersonConfiguration)
	{
		trustKeyPersonConfiguration.ToTable("ApplicationFormTrustKeyPerson", DEFAULT_SCHEMA);
		trustKeyPersonConfiguration.HasKey(a => a.Id);
		trustKeyPersonConfiguration.Property<int?>("ApplicationFormTrustId").IsRequired(false);

		trustKeyPersonConfiguration
			.HasMany(a => a.Roles)
			.WithOne()
			.HasForeignKey("ApplicationFormTrustKeyPersonRoleId")
			.IsRequired();

		var navigation = trustKeyPersonConfiguration.Metadata.FindNavigation(nameof(TrustKeyPerson.Roles));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureTrustKeyPersonRole(EntityTypeBuilder<TrustKeyPersonRole> trustKeyPersonRoleConfiguration)
	{
		trustKeyPersonRoleConfiguration.ToTable("ApplicationFormTrustKeyPersonRole", DEFAULT_SCHEMA);
		trustKeyPersonRoleConfiguration.HasKey(a => a.Id);
		trustKeyPersonRoleConfiguration.Property<int?>("ApplicationFormTrustKeyPersonRoleId").IsRequired(false);
	}
}
