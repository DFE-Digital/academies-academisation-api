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

	public CommandResult UpdatePatch(ProjectDetails detailsToUpdate)
	{
		Details = new ProjectDetails(1, 1)
		{
			HeadTeacherBoardDate = Details.HeadTeacherBoardDate == default(DateTime)
			   ? null
			   : detailsToUpdate.HeadTeacherBoardDate ?? Details.HeadTeacherBoardDate,
			Author = detailsToUpdate.Author ?? Details.Author,
			ClearedBy = detailsToUpdate.ClearedBy ?? Details.ClearedBy,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate ?? Details.ProposedAcademyOpeningDate,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber ?? Details.PublishedAdmissionNumber,
			ViabilityIssues = detailsToUpdate.ViabilityIssues ?? Details.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit ?? Details.FinancialDeficit,
			RationaleForProject = detailsToUpdate.RationaleForProject ?? Details.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust ?? Details.RationaleForTrust,
			RisksAndIssues = detailsToUpdate.RisksAndIssues ?? Details.RisksAndIssues,
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear ?? Details.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear ?? Details.ProjectedRevenueBalanceAtEndMarchNextYear,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete ?? Details.RationaleSectionComplete,
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate == default(DateTime)
				? null
				: detailsToUpdate.LocalAuthorityInformationTemplateSentDate ?? Details.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate == default(DateTime)
				? null
				: detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate ?? Details.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments ?? Details.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink ?? Details.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete ?? Details.LocalAuthorityInformationTemplateSectionComplete,
			RecommendationForProject = detailsToUpdate.RecommendationForProject ?? Details.RecommendationForProject,
			AcademyOrderRequired = detailsToUpdate.AcademyOrderRequired ?? Details.AcademyOrderRequired,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete ?? Details.SchoolAndTrustInformationSectionComplete,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters ?? Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation ?? Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentName = detailsToUpdate.MemberOfParliamentName ?? Details.MemberOfParliamentName,
			MemberOfParliamentParty = detailsToUpdate.MemberOfParliamentParty ?? Details.MemberOfParliamentParty,
			GeneralInformationSectionComplete = detailsToUpdate.GeneralInformationSectionComplete ?? Details.GeneralInformationSectionComplete,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete ?? Details.RisksAndIssuesSectionComplete,
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation ?? Details.SchoolPerformanceAdditionalInformation,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear ?? Details.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear ?? Details.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation ?? Details.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete ?? Details.SchoolBudgetInformationSectionComplete,
			SchoolPupilForecastsAdditionalInformation = detailsToUpdate.SchoolPupilForecastsAdditionalInformation ?? Details.SchoolPupilForecastsAdditionalInformation,
			YearOneProjectedCapacity = detailsToUpdate.YearOneProjectedCapacity ?? Details.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = detailsToUpdate.YearOneProjectedPupilNumbers ?? Details.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = detailsToUpdate.YearTwoProjectedCapacity ?? Details.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = detailsToUpdate.YearTwoProjectedPupilNumbers ?? Details.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = detailsToUpdate.YearThreeProjectedCapacity ?? Details.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = detailsToUpdate.YearThreeProjectedPupilNumbers ?? Details.YearThreeProjectedPupilNumbers,
			KeyStage2PerformanceAdditionalInformation = detailsToUpdate.KeyStage2PerformanceAdditionalInformation ??
			Details.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = detailsToUpdate.KeyStage4PerformanceAdditionalInformation ?? Details.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = detailsToUpdate.KeyStage5PerformanceAdditionalInformation ?? Details.KeyStage5PerformanceAdditionalInformation,
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion ?? Details.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate == default(DateTime)
				? null
				: detailsToUpdate.PreviousHeadTeacherBoardDate ?? Details.PreviousHeadTeacherBoardDate,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount ?? Details.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason ?? Details.ConversionSupportGrantChangeReason,
			ProjectStatus = detailsToUpdate.ProjectStatus ?? Details.ProjectStatus
		};		

		return new CommandSuccessResult();
	}

	private static string? ToYesNoString(bool? value)
	{
		if (!value.HasValue) return null;
		return value == true ? "Yes" : "No";
	}
}
