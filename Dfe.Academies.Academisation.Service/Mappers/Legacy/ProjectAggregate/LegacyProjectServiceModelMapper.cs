using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectServiceModelMapper
{
	internal static LegacyProjectServiceModel MapToServiceModel(this IProject project)
	{
		LegacyProjectServiceModel serviceModel = new(
			project.Id,
			project.Details.Urn
			)
		{
			SchoolName = project.Details.SchoolName,
			LocalAuthority = project.Details.LocalAuthority,
			ApplicationReferenceNumber = project.Details.ApplicationReferenceNumber,
			ProjectStatus = project.Details.ProjectStatus,
			ApplicationReceivedDate = project.Details.ApplicationReceivedDate,
			AssignedDate = project.Details.AssignedDate,
			HeadTeacherBoardDate = project.Details.HeadTeacherBoardDate,
			OpeningDate = project.Details.OpeningDate,
			BaselineDate = project.Details.BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = project.Details.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = project.Details.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = project.Details.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = project.Details.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = project.Details.LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = project.Details.RecommendationForProject,
			Author = project.Details.Author,
			Version = project.Details.Version,
			ClearedBy = project.Details.ClearedBy,
			AcademyOrderRequired = project.Details.AcademyOrderRequired,
			PreviousHeadTeacherBoardDateQuestion = project.Details.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = project.Details.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = project.Details.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = project.Details.TrustReferenceNumber,
			NameOfTrust = project.Details.NameOfTrust,
			SponsorReferenceNumber = project.Details.SponsorReferenceNumber,
			SponsorName = project.Details.SponsorName,
			AcademyTypeAndRoute = project.Details.AcademyTypeAndRoute,
			ProposedAcademyOpeningDate = project.Details.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = project.Details.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = project.Details.ConversionSupportGrantAmount,  // had to make this nullable or move it to the top
			ConversionSupportGrantChangeReason = project.Details.ConversionSupportGrantChangeReason,

			// general info
			PublishedAdmissionNumber = project.Details.PublishedAdmissionNumber,
			PartOfPfiScheme = project.Details.PartOfPfiScheme,
			ViabilityIssues = project.Details.ViabilityIssues,
			FinancialDeficit = project.Details.FinancialDeficit,
			DistanceFromSchoolToTrustHeadquarters = project.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = project.Details.MemberOfParliamentParty,
			MemberOfParliamentName = project.Details.MemberOfParliamentName,

			GeneralInformationSectionComplete = project.Details.GeneralInformationSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = project.Details.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = project.Details.RationaleForProject,
			RationaleForTrust = project.Details.RationaleForTrust,
			RationaleSectionComplete = project.Details.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = project.Details.RisksAndIssues,
			EqualitiesImpactAssessmentConsidered = project.Details.EqualitiesImpactAssessmentConsidered,
			RisksAndIssuesSectionComplete = project.Details.RisksAndIssuesSectionComplete,

			// school budget info
			RevenueCarryForwardAtEndMarchCurrentYear = project.Details.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = project.Details.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = project.Details.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = project.Details.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = project.Details.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = project.Details.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			//CurrentYearCapacity = project.Details.CurrentYearCapacity,
			//CurrentYearPupilNumbers = project.Details.CurrentYearPupilNumbers,
			YearOneProjectedCapacity = project.Details.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = project.Details.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = project.Details.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = project.Details.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = project.Details.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = project.Details.YearThreeProjectedPupilNumbers,
			SchoolPupilForecastsAdditionalInformation = project.Details.SchoolPupilForecastsAdditionalInformation,

			// key stage performance tables
			KeyStage2PerformanceAdditionalInformation = project.Details.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = project.Details.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = project.Details.KeyStage5PerformanceAdditionalInformation,
			Upin = project.Details.Upin,
			NewAcademyUrn = project.Details.NewAcademyUrn
		};

		return serviceModel;
	}
}
