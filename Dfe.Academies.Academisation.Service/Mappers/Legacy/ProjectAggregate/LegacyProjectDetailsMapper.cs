using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectDetailsMapper
{
	internal static ProjectDetails MapNonEmptyFields(this LegacyProjectServiceModel detailsToUpdate, IProject existingProject)
	{
		return new(detailsToUpdate.Urn ?? existingProject.Details.Urn, 1)
		{
			HeadTeacherBoardDate = detailsToUpdate.HeadTeacherBoardDate == default(DateTime)
			   ? null
			   : detailsToUpdate.HeadTeacherBoardDate ?? existingProject.Details.HeadTeacherBoardDate,
			Author = detailsToUpdate.Author ?? existingProject.Details.Author,
			ClearedBy = detailsToUpdate.ClearedBy ?? existingProject.Details.ClearedBy,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate ?? existingProject.Details.ProposedAcademyOpeningDate,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber ?? existingProject.Details.PublishedAdmissionNumber,
			ViabilityIssues = detailsToUpdate.ViabilityIssues ?? existingProject.Details.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit ?? existingProject.Details.FinancialDeficit,
			RationaleForProject = detailsToUpdate.RationaleForProject ?? existingProject.Details.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust ?? existingProject.Details.RationaleForTrust,
			RisksAndIssues = detailsToUpdate.RisksAndIssues ?? existingProject.Details.RisksAndIssues,
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear ?? existingProject.Details.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear ?? existingProject.Details.ProjectedRevenueBalanceAtEndMarchNextYear,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete ?? existingProject.Details.RationaleSectionComplete,
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate == default(DateTime)
				? null
				: detailsToUpdate.LocalAuthorityInformationTemplateSentDate ?? existingProject.Details.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate == default(DateTime)
				? null
				: detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate ?? existingProject.Details.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments ?? existingProject.Details.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink ?? existingProject.Details.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete ?? existingProject.Details.LocalAuthorityInformationTemplateSectionComplete,
			RecommendationForProject = detailsToUpdate.RecommendationForProject ?? existingProject.Details.RecommendationForProject,
			AcademyOrderRequired = detailsToUpdate.AcademyOrderRequired ?? existingProject.Details.AcademyOrderRequired,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete ?? existingProject.Details.SchoolAndTrustInformationSectionComplete,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters ?? existingProject.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation ?? existingProject.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentName = detailsToUpdate.MemberOfParliamentName ?? existingProject.Details.MemberOfParliamentName,
			MemberOfParliamentParty = detailsToUpdate.MemberOfParliamentParty ?? existingProject.Details.MemberOfParliamentParty,
			GeneralInformationSectionComplete = detailsToUpdate.GeneralInformationSectionComplete ?? existingProject.Details.GeneralInformationSectionComplete,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete ?? existingProject.Details.RisksAndIssuesSectionComplete,
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation ?? existingProject.Details.SchoolPerformanceAdditionalInformation,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear ?? existingProject.Details.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear ?? existingProject.Details.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation ?? existingProject.Details.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete ?? existingProject.Details.SchoolBudgetInformationSectionComplete,
			SchoolPupilForecastsAdditionalInformation = detailsToUpdate.SchoolPupilForecastsAdditionalInformation ?? existingProject.Details.SchoolPupilForecastsAdditionalInformation,
			YearOneProjectedCapacity = detailsToUpdate.YearOneProjectedCapacity ?? existingProject.Details.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = detailsToUpdate.YearOneProjectedPupilNumbers ?? existingProject.Details.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = detailsToUpdate.YearTwoProjectedCapacity ?? existingProject.Details.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = detailsToUpdate.YearTwoProjectedPupilNumbers ?? existingProject.Details.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = detailsToUpdate.YearThreeProjectedCapacity ?? existingProject.Details.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = detailsToUpdate.YearThreeProjectedPupilNumbers ?? existingProject.Details.YearThreeProjectedPupilNumbers,
			KeyStage2PerformanceAdditionalInformation = detailsToUpdate.KeyStage2PerformanceAdditionalInformation ??
			existingProject.Details.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = detailsToUpdate.KeyStage4PerformanceAdditionalInformation ?? existingProject.Details.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = detailsToUpdate.KeyStage5PerformanceAdditionalInformation ?? existingProject.Details.KeyStage5PerformanceAdditionalInformation,
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion ?? existingProject.Details.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate == default(DateTime)
				? null
				: detailsToUpdate.PreviousHeadTeacherBoardDate ?? existingProject.Details.PreviousHeadTeacherBoardDate,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount ?? existingProject.Details.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason ?? existingProject.Details.ConversionSupportGrantChangeReason,
			ProjectStatus = detailsToUpdate.ProjectStatus ?? existingProject.Details.ProjectStatus
		};				
	}
}
