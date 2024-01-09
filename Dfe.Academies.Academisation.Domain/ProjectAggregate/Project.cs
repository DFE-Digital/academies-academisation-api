using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project : Entity, IProject, IAggregateRoot
{
	protected Project() { }

	private Project(ProjectDetails projectDetails)
	{
		Details = projectDetails;

	}

	public IEnumerable<ProjectNote> Notes => _notes.AsReadOnly();
	IReadOnlyCollection<IProjectNote> IProject.Notes => _notes.AsReadOnly();

	private readonly List<ProjectNote> _notes = new();

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails, DateTime? createdOn = null)
	{
		Id = id;
		Details = projectDetails;
		CreatedOn = createdOn ?? DateTime.Now;
	}

	public ProjectDetails Details { get; private set; }

	// Create from A2b 
	public static CreateResult Create(IApplication application)
	{
		if (application.ApplicationType != ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult(
				new List<ValidationError>
				{
					new("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var school = application.Schools.Single().Details;

		var projectDetails = new ProjectDetails
		{
			Urn = school.Urn,
			SchoolName = school.SchoolName,
			ApplicationReferenceNumber = $"A2B_{application.ApplicationId}",
			ProjectStatus = "Converter Pre-AO (C)",
			ApplicationReceivedDate = application.ApplicationSubmittedDate,
			TrustReferenceNumber = application.JoinTrust?.TrustReference,
			NameOfTrust = application.JoinTrust?.TrustName,
			AcademyTypeAndRoute = "Converter",
			// Temp hotfix
			ProposedAcademyOpeningDate = null,
			ConversionSupportGrantAmount = 25000,
			PublishedAdmissionNumber = school.CapacityPublishedAdmissionsNumber.ToString(),
			PartOfPfiScheme = ToYesNoString(school.LandAndBuildings?.PartOfPfiScheme),
			FinancialDeficit = ToYesNoString(IsDeficit(school.CurrentFinancialYear?.CapitalCarryForwardStatus)),
			RationaleForTrust = school.SchoolConversionReasonsForJoining,
			EndOfCurrentFinancialYear = school.CurrentFinancialYear?.FinancialYearEndDate,
			EndOfNextFinancialYear = school.NextFinancialYear?.FinancialYearEndDate,
			RevenueCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.CurrentFinancialYear?.Revenue, school.CurrentFinancialYear?.RevenueStatus),
			ProjectedRevenueBalanceAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.NextFinancialYear?.Revenue, school.NextFinancialYear?.RevenueStatus),
			CapitalCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.CurrentFinancialYear?.CapitalCarryForward, school.CurrentFinancialYear?.CapitalCarryForwardStatus),
			CapitalCarryForwardAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.NextFinancialYear?.CapitalCarryForward, school.NextFinancialYear?.CapitalCarryForwardStatus),
			YearOneProjectedPupilNumbers = school.ProjectedPupilNumbersYear1,
			YearTwoProjectedPupilNumbers = school.ProjectedPupilNumbersYear2,
			YearThreeProjectedPupilNumbers = school.ProjectedPupilNumbersYear3
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}

	public static CreateResult CreateFormAMat(IApplication application)
	{
		if (application.ApplicationType != ApplicationType.FormAMat)
		{
			return new CreateValidationErrorResult(
				new List<ValidationError>
				{
					new("ApplicationStatus", "Only projects of type FormAMat are supported")
				});
		}

		var projectDetailsList = application.Schools.Select(school => new ProjectDetails
		{
			Urn = school.Details.Urn,
			SchoolName = school.Details.SchoolName,
			ApplicationReferenceNumber = $"A2B_{application.ApplicationId}",
			ProjectStatus = "Converter Pre-AO (C)",
			ApplicationReceivedDate = application.ApplicationSubmittedDate,
			NameOfTrust = application.FormTrust?.TrustDetails.FormTrustProposedNameOfTrust,
			AcademyTypeAndRoute = "Form a Mat",
			// Temp hotfix
			ProposedAcademyOpeningDate = null,
			ConversionSupportGrantAmount = 25000,
			PublishedAdmissionNumber = school.Details.CapacityPublishedAdmissionsNumber.ToString(),
			PartOfPfiScheme = ToYesNoString(school.Details.LandAndBuildings?.PartOfPfiScheme),
			FinancialDeficit = ToYesNoString(IsDeficit(school.Details.CurrentFinancialYear?.CapitalCarryForwardStatus)),
			RationaleForTrust = school.Details.SchoolConversionReasonsForJoining,
			EndOfCurrentFinancialYear = school.Details.CurrentFinancialYear?.FinancialYearEndDate,
			EndOfNextFinancialYear = school.Details.NextFinancialYear?.FinancialYearEndDate,
			RevenueCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.Details.CurrentFinancialYear?.Revenue, school.Details.CurrentFinancialYear?.RevenueStatus),
			ProjectedRevenueBalanceAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.Details.NextFinancialYear?.Revenue, school.Details.NextFinancialYear?.RevenueStatus),
			CapitalCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.Details.CurrentFinancialYear?.CapitalCarryForward, school.Details.CurrentFinancialYear?.CapitalCarryForwardStatus),
			CapitalCarryForwardAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.Details.NextFinancialYear?.CapitalCarryForward, school.Details.NextFinancialYear?.CapitalCarryForwardStatus),
			YearOneProjectedPupilNumbers = school.Details.ProjectedPupilNumbersYear1,
			YearTwoProjectedPupilNumbers = school.Details.ProjectedPupilNumbersYear2,
			YearThreeProjectedPupilNumbers = school.Details.ProjectedPupilNumbersYear3
		})
			.ToList();

		var projectList = projectDetailsList.Select(projectDetails => new Project(projectDetails)).ToList();
		return new CreateSuccessResult<IEnumerable<IProject>>(projectList);
	}
	// Create from Conversions
	public static CreateResult CreateNewProject(NewProject project)
	{
		ArgumentNullException.ThrowIfNull(project);

		if (project.Trust == null)
		{
			return new CreateValidationErrorResult(new List<ValidationError>
			{
				new("Trust", "Trust in the model must not be null")
			});
		}
		if (project.School == null)
		{
			return new CreateValidationErrorResult(new List<ValidationError>
			{
				new("School", "School in the model must not be null")
			});
		}

		var projectDetails = new ProjectDetails
		{
			Urn = project.School.Urn,
			SchoolName = project.School?.Name,
			ProjectStatus = "Converter Pre-AO (C)",
			TrustReferenceNumber = project.Trust?.ReferenceNumber,
			NameOfTrust = project.Trust?.Name,
			AcademyTypeAndRoute = DetermineRoute(project),
			ConversionSupportGrantAmount = 25000,
			PartOfPfiScheme = ToYesNoString(project.School?.PartOfPfiScheme) ?? "No",
			LocalAuthority = project.School?.LocalAuthorityName,
			Region = project.School?.Region
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}

	public static string DetermineRoute(NewProject project)
	{
		return project.HasSchoolApplied?.ToLower() switch
		{
			"yes" => "Converter",
			"no" => "Sponsored",
			_ => "Converter",
		};
	}

	public CommandResult Update(ProjectDetails detailsToUpdate)
	{
		if (Details.Urn != detailsToUpdate.Urn)
		{
			return new CommandValidationErrorResult(new List<ValidationError>
			{
				new("Urn", "Urn in update model must match existing record")
			});
		}

		Details = new ProjectDetails
		{
			Urn = detailsToUpdate.Urn,
			IfdPipelineId = detailsToUpdate.IfdPipelineId,
			SchoolName = detailsToUpdate.SchoolName,
			LocalAuthority = detailsToUpdate.LocalAuthority,
			ApplicationReferenceNumber = detailsToUpdate.ApplicationReferenceNumber,
			ProjectStatus = detailsToUpdate.ProjectStatus,
			ApplicationReceivedDate = detailsToUpdate.ApplicationReceivedDate,
			AssignedDate = detailsToUpdate.AssignedDate,
			HeadTeacherBoardDate = detailsToUpdate.HeadTeacherBoardDate,
			BaselineDate = detailsToUpdate.BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = detailsToUpdate.RecommendationForProject,
			Author = detailsToUpdate.Author,
			Version = detailsToUpdate.Version,
			ClearedBy = detailsToUpdate.ClearedBy,
			AcademyOrderRequired = detailsToUpdate.AcademyOrderRequired,
			DaoPackSentDate = detailsToUpdate.DaoPackSentDate,
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = detailsToUpdate.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = detailsToUpdate.TrustReferenceNumber,
			NameOfTrust = detailsToUpdate.NameOfTrust,
			SponsorReferenceNumber = detailsToUpdate.SponsorReferenceNumber,
			SponsorName = detailsToUpdate.SponsorName,
			AcademyTypeAndRoute = detailsToUpdate.AcademyTypeAndRoute,
			Form7Received = detailsToUpdate.Form7Received,
			Form7ReceivedDate = detailsToUpdate.Form7ReceivedDate,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = CalculateDefaultSponsoredGrant(Details.ConversionSupportGrantType, detailsToUpdate.ConversionSupportGrantType, detailsToUpdate.ConversionSupportGrantAmount, detailsToUpdate.ConversionSupportGrantAmountChanged, detailsToUpdate.SchoolPhase ?? Details.SchoolPhase),
			ConversionSupportGrantChangeReason = NullifyGrantChangeReasonIfNeeded(detailsToUpdate.ConversionSupportGrantAmountChanged, detailsToUpdate.ConversionSupportGrantChangeReason, detailsToUpdate.AcademyTypeAndRoute),
			ConversionSupportGrantType = detailsToUpdate.ConversionSupportGrantType,
			ConversionSupportGrantEnvironmentalImprovementGrant = detailsToUpdate.ConversionSupportGrantEnvironmentalImprovementGrant,
			ConversionSupportGrantAmountChanged = detailsToUpdate.ConversionSupportGrantAmountChanged,
			ConversionSupportGrantNumberOfSites = detailsToUpdate.ConversionSupportGrantNumberOfSites,
			Region = detailsToUpdate.Region,

			// Annex B
			AnnexBFormReceived = detailsToUpdate.AnnexBFormReceived,
			AnnexBFormUrl = detailsToUpdate.AnnexBFormUrl,

			// External Application Form
			ExternalApplicationFormSaved = detailsToUpdate.ExternalApplicationFormSaved,
			ExternalApplicationFormUrl = detailsToUpdate.ExternalApplicationFormUrl,

			// School Overview
			SchoolPhase = detailsToUpdate.SchoolPhase,
			AgeRange = detailsToUpdate.AgeRange,
			SchoolType = detailsToUpdate.SchoolType,
			ActualPupilNumbers = detailsToUpdate.ActualPupilNumbers,
			Capacity = detailsToUpdate.Capacity,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = detailsToUpdate.PercentageFreeSchoolMeals,
			PartOfPfiScheme = detailsToUpdate.PartOfPfiScheme,
			PfiSchemeDetails = detailsToUpdate.PfiSchemeDetails,
			ViabilityIssues = detailsToUpdate.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit,
			NumberOfPlacesFundedFor = detailsToUpdate.NumberOfPlacesFundedFor,
			NumberOfResidentialPlaces = detailsToUpdate.NumberOfResidentialPlaces,
			NumberOfFundedResidentialPlaces = detailsToUpdate.NumberOfFundedResidentialPlaces,
			DiocesanTrust = detailsToUpdate.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = detailsToUpdate.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentNameAndParty = detailsToUpdate.MemberOfParliamentNameAndParty,

			SchoolOverviewSectionComplete = detailsToUpdate.SchoolOverviewSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = detailsToUpdate.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = detailsToUpdate.RisksAndIssues,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete,

			// legal requirements
			GoverningBodyResolution = detailsToUpdate.GoverningBodyResolution,
			Consultation = detailsToUpdate.Consultation,
			DiocesanConsent = detailsToUpdate.DiocesanConsent,
			FoundationConsent = detailsToUpdate.FoundationConsent,
			LegalRequirementsSectionComplete = detailsToUpdate.LegalRequirementsSectionComplete,

			// school budget info
			EndOfCurrentFinancialYear = detailsToUpdate.EndOfCurrentFinancialYear,
			EndOfNextFinancialYear = detailsToUpdate.EndOfNextFinancialYear,
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			YearOneProjectedCapacity = detailsToUpdate.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = detailsToUpdate.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = detailsToUpdate.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = detailsToUpdate.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = detailsToUpdate.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = detailsToUpdate.YearThreeProjectedPupilNumbers,
			SchoolPupilForecastsAdditionalInformation = detailsToUpdate.SchoolPupilForecastsAdditionalInformation,

			// key stage performance tables
			KeyStage2PerformanceAdditionalInformation = detailsToUpdate.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = detailsToUpdate.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = detailsToUpdate.KeyStage5PerformanceAdditionalInformation,

			// assigned users
			AssignedUser = MapUser(detailsToUpdate.AssignedUser)
		};

		return new CommandSuccessResult();
	}

	private static bool? IsDeficit(RevenueType? revenueType)
	{
		return revenueType.HasValue
			? revenueType == RevenueType.Deficit
			: null;
	}

	private static decimal? ConvertDeficitAmountToNegative(decimal? amount, RevenueType? revenueType)
	{
		if (revenueType.HasValue)
		{
			return IsDeficit(revenueType)!.Value ? amount * -1.0M : amount;
		}
		return null;
	}

	private static string? ToYesNoString(bool? value)
	{
		if (value.HasValue is false) return null;
		return value == true ? "Yes" : "No";
	}

	private static User? MapUser(User? user)
	{
		if (user == null) return null;
		return new User(user.Id, user.FullName, user.EmailAddress);
	}

	// Sponsored grant logic
	private const string FastTrackGrantType = "fast track";
	private const string IntermediateGrantType = "intermediate";
	private const string FullGrantType = "full";
	const decimal FastTrackDefaultPrimary = 70000;
	const decimal FastTrackDefaultSecondary = 80000;
	const decimal IntermediateDefaultPrimary = 90000;
	const decimal IntermediateDefaultSecondary = 115000;
	const decimal FullDefaultPrimary = 110000;
	const decimal FullDefaultSecondary = 150000;
	public static decimal? CalculateDefaultSponsoredGrant(string? existingConversionSupportGrantType,
		string? newConversionSupportGrantType,
		decimal? currentGrantAmount,
		bool? detailsConversionSupportGrantAmountChanged,
		string? schoolPhase)
	{
		if (schoolPhase != null)
		{
			switch (schoolPhase.ToLower())
			{
				case "secondary":
					return CalculateDefaultSponsoredGrantForSecondary(existingConversionSupportGrantType,
						newConversionSupportGrantType, currentGrantAmount, detailsConversionSupportGrantAmountChanged);
				case "primary":
					return CalculateDefaultSponsoredGrantForPrimary(existingConversionSupportGrantType,
						newConversionSupportGrantType, currentGrantAmount, detailsConversionSupportGrantAmountChanged);
			}
		}

		// if it's the same type remain unchanged
		if (existingConversionSupportGrantType == newConversionSupportGrantType)
		{
			return currentGrantAmount;
		}

		return currentGrantAmount;
	}

	private static decimal? CalculateDefaultSponsoredGrantForPrimary(string? existingConversionSupportGrantType,
		string? newConversionSupportGrantType, decimal? currentGrantAmount,
		bool? detailsConversionSupportGrantAmountChanged)
	{
		// if the amount has been flagged as "keeping default" reset any previous changes to ensure default value
		if (detailsConversionSupportGrantAmountChanged is true)
		{
			return DetermineValueFromTypePrimary(newConversionSupportGrantType, currentGrantAmount);
		}

		// if it's empty and now becoming a type, set the default
		if (string.IsNullOrEmpty(existingConversionSupportGrantType))
		{
			return DetermineValueFromTypePrimary(newConversionSupportGrantType, currentGrantAmount);
		}

		// if it's changed type set the new default
		if (existingConversionSupportGrantType != newConversionSupportGrantType)
		{
			return DetermineValueFromTypePrimary(newConversionSupportGrantType, currentGrantAmount);
		}

		return currentGrantAmount;
	}
	private static decimal? CalculateDefaultSponsoredGrantForSecondary(string? existingConversionSupportGrantType,
		string? newConversionSupportGrantType, decimal? currentGrantAmount,
		bool? detailsConversionSupportGrantAmountChanged)
	{
		// if the amount has been flagged as "keeping default" reset any previous changes to ensure default value
		if (detailsConversionSupportGrantAmountChanged is true)
		{
			return DetermineValueFromTypeSecondary(newConversionSupportGrantType, currentGrantAmount);
		}

		// if it's empty and now becoming a type, set the default
		if (string.IsNullOrEmpty(existingConversionSupportGrantType))
		{
			return DetermineValueFromTypeSecondary(newConversionSupportGrantType, currentGrantAmount);
		}

		// if it's changed type set the new default
		if (existingConversionSupportGrantType != newConversionSupportGrantType)
		{
			return DetermineValueFromTypeSecondary(newConversionSupportGrantType, currentGrantAmount);
		}

		return currentGrantAmount;
	}

	private static decimal? DetermineValueFromTypePrimary(string? grantType, decimal? currentAmount)
	{
		return grantType?.ToLower() switch
		{
			FastTrackGrantType => FastTrackDefaultPrimary,
			IntermediateGrantType => IntermediateDefaultPrimary,
			FullGrantType => FullDefaultPrimary,
			_ => currentAmount
		};
	}
	private static decimal? DetermineValueFromTypeSecondary(string? grantType, decimal? currentAmount)
	{
		return grantType?.ToLower() switch
		{
			FastTrackGrantType => FastTrackDefaultSecondary,
			IntermediateGrantType => IntermediateDefaultSecondary,
			FullGrantType => FullDefaultSecondary,
			_ => currentAmount
		};
	}
	protected string? NullifyGrantChangeReasonIfNeeded(bool? grantAmountChanged, string? reason, string? route)
	{
		if (route == "Sponsored")
		{
			return grantAmountChanged switch
			{
				true => null,
				false => reason,
				_ => reason
			};
		}

		return reason;
	}

	public void SetExternalApplicationForm(bool ExternalApplicationFormSaved, string ExternalApplicationFormUrl)
	{
		this.Details.ExternalApplicationFormSaved = ExternalApplicationFormSaved;
		this.Details.ExternalApplicationFormUrl = ExternalApplicationFormUrl;
	}
	public void SetSchoolOverview(
							  string publishedAdmissionNumber,
							  string viabilityIssues,
							  string partOfPfiScheme,
							  string financialDeficit,
							  decimal? numberOfPlacesFundedFor,
							  decimal? numberOfResidentialPlaces,
							  decimal? numberOfFundedResidentialPlaces,
							  string pfiSchemeDetails,
							  decimal? distanceFromSchoolToTrustHeadquarters,
							  string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
							  string memberOfParliamentNameAndParty
		)
	{
		// Update the respective properties in the Details object
		this.Details.PublishedAdmissionNumber = publishedAdmissionNumber;
		this.Details.ViabilityIssues = viabilityIssues;
		this.Details.PartOfPfiScheme = partOfPfiScheme;
		this.Details.FinancialDeficit = financialDeficit;
		this.Details.NumberOfPlacesFundedFor = numberOfPlacesFundedFor;
		this.Details.NumberOfResidentialPlaces = numberOfResidentialPlaces;
		this.Details.NumberOfFundedResidentialPlaces = numberOfFundedResidentialPlaces;
		this.Details.PfiSchemeDetails = pfiSchemeDetails;
		this.Details.DistanceFromSchoolToTrustHeadquarters = distanceFromSchoolToTrustHeadquarters;
		this.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation = distanceFromSchoolToTrustHeadquartersAdditionalInformation;
		this.Details.MemberOfParliamentNameAndParty = memberOfParliamentNameAndParty;

		// Update the LastModifiedOn property to the current time to indicate the object has been modified
		this.LastModifiedOn = DateTime.UtcNow;
	}

}
