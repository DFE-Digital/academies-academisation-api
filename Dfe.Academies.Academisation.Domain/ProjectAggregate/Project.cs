using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project : IProject
{
	private Project(ProjectDetails projectDetails)
	{
		Details = projectDetails;
	}

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails)
	{
		Id = id;
		Details = projectDetails;
	}

	public int Id { get; }

	public ProjectDetails Details { get; private set; }


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
			OpeningDate = DateTime.Today.AddMonths(6),
			TrustReferenceNumber = application.JoinTrust?.TrustReference,
			NameOfTrust = application.JoinTrust?.TrustName,
			AcademyTypeAndRoute = "Converter",
			ProposedAcademyOpeningDate = school.ConversionTargetDate,
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
				OpeningDate = DateTime.Today.AddMonths(6),
				NameOfTrust = application.FormTrust?.TrustDetails.FormTrustProposedNameOfTrust,
				AcademyTypeAndRoute = "Form a Mat",
				ProposedAcademyOpeningDate = school.Details.ConversionTargetDate,
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

	public static CreateResult CreateSponsoredProject(SponsoredProject project)
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
			OpeningDate = DateTime.Today.AddMonths(6),
			TrustReferenceNumber = project.Trust?.ReferenceNumber,
			NameOfTrust = project.Trust?.Name,
			AcademyTypeAndRoute = "Sponsored",
			ConversionSupportGrantAmount = 25000,
			PartOfPfiScheme = ToYesNoString(project.School?.PartOfPfiScheme) ?? "No",
			LocalAuthority = project.School?.LocalAuthorityName,
			Region = project.School?.Region
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
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
			OpeningDate = detailsToUpdate.OpeningDate,
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
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason,
			Region	= detailsToUpdate.Region,

			// Annex B
			AnnexBFormReceived = detailsToUpdate.AnnexBFormReceived,
			AnnexBFormUrl = detailsToUpdate.AnnexBFormUrl,

			// general info
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
			DiocesanTrust = detailsToUpdate.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = detailsToUpdate.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = detailsToUpdate.MemberOfParliamentParty,
			MemberOfParliamentName = detailsToUpdate.MemberOfParliamentName,

			GeneralInformationSectionComplete = detailsToUpdate.GeneralInformationSectionComplete,

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
}
