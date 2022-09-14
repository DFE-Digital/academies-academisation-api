using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectDetailsMapper
{
	internal static ProjectDetails ToDomain(this LegacyProjectServiceModel serviceModel)
	{
		return new(serviceModel.Urn, 1)
		{
			HeadTeacherBoardDate = serviceModel.HeadTeacherBoardDate,
			Author = serviceModel.Author,
			ClearedBy = serviceModel.ClearedBy,
			ProposedAcademyOpeningDate = serviceModel.ProposedAcademyOpeningDate,
			PublishedAdmissionNumber = serviceModel.PublishedAdmissionNumber,
			ViabilityIssues = serviceModel.ViabilityIssues,
			FinancialDeficit = serviceModel.FinancialDeficit ,
			RationaleForProject = serviceModel.RationaleForProject,
			RationaleForTrust = serviceModel.RationaleForTrust ,
			RisksAndIssues = serviceModel.RisksAndIssues,
			RevenueCarryForwardAtEndMarchCurrentYear = serviceModel.RevenueCarryForwardAtEndMarchCurrentYear ,
			ProjectedRevenueBalanceAtEndMarchNextYear = serviceModel.ProjectedRevenueBalanceAtEndMarchNextYear,
			RationaleSectionComplete = serviceModel.RationaleSectionComplete,
			LocalAuthorityInformationTemplateReturnedDate = serviceModel.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = serviceModel.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = serviceModel.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = serviceModel.LocalAuthorityInformationTemplateSectionComplete,
			RecommendationForProject = serviceModel.RecommendationForProject,
			SchoolAndTrustInformationSectionComplete = serviceModel.SchoolAndTrustInformationSectionComplete,
			DistanceFromSchoolToTrustHeadquarters = serviceModel.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = serviceModel.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = serviceModel.MemberOfParliamentParty,
			GeneralInformationSectionComplete = serviceModel.GeneralInformationSectionComplete,
			RisksAndIssuesSectionComplete = serviceModel.RisksAndIssuesSectionComplete,
			SchoolPerformanceAdditionalInformation = serviceModel.SchoolPerformanceAdditionalInformation,
			CapitalCarryForwardAtEndMarchCurrentYear = serviceModel.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = serviceModel.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = serviceModel.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = serviceModel.SchoolBudgetInformationSectionComplete,
			SchoolPupilForecastsAdditionalInformation = serviceModel.SchoolPupilForecastsAdditionalInformation,
			YearOneProjectedCapacity = serviceModel.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = serviceModel.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = serviceModel.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = serviceModel.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = serviceModel.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = serviceModel.YearThreeProjectedPupilNumbers,
			KeyStage2PerformanceAdditionalInformation = serviceModel.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = serviceModel.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = serviceModel.KeyStage5PerformanceAdditionalInformation,
			PreviousHeadTeacherBoardDateQuestion = serviceModel.PreviousHeadTeacherBoardDateQuestion,
			ConversionSupportGrantAmount = serviceModel.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = serviceModel.ConversionSupportGrantChangeReason,
			ProjectStatus = serviceModel.ProjectStatus
		};				
	}
}
