using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
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

	public static CreateResult<IProject> Create(IApplication application, EstablishmentDetails establishmentDetails, MisEstablishmentDetails misEstablishmentDetails)
	{
		if (application.ApplicationType != Core.ApplicationAggregate.ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult<IProject>(
				new List<ValidationError>
				{
					new ValidationError("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var school = application.Schools.Single().Details;

		var projectDetails = new ProjectDetails(
			school.Urn,
			misEstablishmentDetails.Laestab
		)
		{
			UkPrn = establishmentDetails.Ukprn,
			// TODO: map additional fields as they become available
			//LocalAuthority = school.LocalAuthority.LocalAuthorityName,
			//ApplicationReferenceNumber = application.ApplicationId
			ProjectStatus = "Converter Pre-AO (C)",
			//ApplicationReceivedDate = application.ApplicationSubmittedOn
			OpeningDate = DateTime.Today.AddMonths(6),
			//TrustReferenceNumber = application.ExistingTrust.ReferenceNumber
			//NameOfTrust = application.ExistingTrust.TrustName
			AcademyTypeAndRoute = "Converter",
			ProposedAcademyOpeningDate = school.ConversionTargetDate,
			ConversionSupportGrantAmount = 25000,
			PublishedAdmissionNumber = school.CapacityPublishedAdmissionsNumber.ToString(),
			PartOfPfiScheme = ToYesNoString(school.LandAndBuildings.PartOfPfiScheme),
			//FinancialDeficit = ToYesNoString(school.SchoolCFYCapitalIsDeficit),
			//RationaleForTrust = school.SchoolConversionReasonsForJoining,
			EqualitiesImpactAssessmentConsidered = ToYesNoString(school.EqualitiesImpactAssessmentCompleted != Core.ApplicationAggregate.EqualityImpact.NotConsidered),
			//SponsorName = application.SponsorName,
			//SponsorReferenceNumber = application.SponsorReferenceNumber,
			//RevenueCarryForwardAtEndMarchCurrentYear = school.SchoolCFYRevenue.ConvertDeficitAmountToNegativeValue(school.SchoolCFYRevenueIsDeficit),
			//ProjectedRevenueBalanceAtEndMarchNextYear = school.SchoolNFYRevenue.ConvertDeficitAmountToNegativeValue(school.SchoolNFYRevenueIsDeficit),
			//CapitalCarryForwardAtEndMarchCurrentYear = school.SchoolCFYCapitalForward.ConvertDeficitAmountToNegativeValue(school.SchoolCFYCapitalIsDeficit),
			//CapitalCarryForwardAtEndMarchNextYear = school.SchoolNFYCapitalForward.ConvertDeficitAmountToNegativeValue(school.SchoolNFYCapitalIsDeficit),
			YearOneProjectedPupilNumbers = school.ProjectedPupilNumbersYear1,
			YearTwoProjectedPupilNumbers = school.ProjectedPupilNumbersYear2,
			YearThreeProjectedPupilNumbers = school.ProjectedPupilNumbersYear3
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}

	public CommandResult Update(ProjectDetails detailsToUpdate)
	{
		if (Details.Urn != detailsToUpdate.Urn)
		{
			return new CommandValidationErrorResult(new List<ValidationError> 
			{ 
				new ValidationError("Urn", "Urn in update model must match existing record") 
			});
		}

		Details = new ProjectDetails(1, detailsToUpdate.Urn)
		{
			Urn = detailsToUpdate.Urn,
			Laestab = detailsToUpdate.Laestab,
			SchoolName = detailsToUpdate.SchoolName,
			LocalAuthority = detailsToUpdate.LocalAuthority,
			ApplicationReferenceNumber = detailsToUpdate.ApplicationReferenceNumber,
			UkPrn = detailsToUpdate.UkPrn,
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
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = detailsToUpdate.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = detailsToUpdate.TrustReferenceNumber,
			NameOfTrust = detailsToUpdate.NameOfTrust,
			SponsorReferenceNumber = detailsToUpdate.SponsorReferenceNumber,
			SponsorName = detailsToUpdate.SponsorName,
			AcademyTypeAndRoute = detailsToUpdate.AcademyTypeAndRoute,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason,

			// general info
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber,
			PartOfPfiScheme = detailsToUpdate.PartOfPfiScheme,
			ViabilityIssues = detailsToUpdate.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit,
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
			EqualitiesImpactAssessmentConsidered = detailsToUpdate.EqualitiesImpactAssessmentConsidered,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete,

			// school budget info
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			CurrentYearCapacity = detailsToUpdate.CurrentYearCapacity,
			CurrentYearPupilNumbers = detailsToUpdate.CurrentYearPupilNumbers,
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
			Upin = detailsToUpdate.Upin,
			NewAcademyUrn = detailsToUpdate.NewAcademyUrn		
		};

		return new CommandSuccessResult();
	}

	private static string? ToYesNoString(bool? value)
	{
		if (!value.HasValue) return null;
		return value == true ? "Yes" : "No";
	}
}
