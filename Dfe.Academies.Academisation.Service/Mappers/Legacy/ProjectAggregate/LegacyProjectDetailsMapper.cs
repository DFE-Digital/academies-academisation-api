using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using ServiceUser = Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate.User;
using DomainUser = Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.User;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectDetailsMapper
{
	internal static ProjectDetails MapNonEmptyFields(this LegacyProjectServiceModel detailsToUpdate, IProject existingProject)
	{
		return new ProjectDetails
		{
			Urn = detailsToUpdate.Urn ?? existingProject.Details.Urn,
			IfdPipelineId = detailsToUpdate.IfdPipelineId ?? existingProject.Details.IfdPipelineId,
			SchoolName = detailsToUpdate.SchoolName ?? existingProject.Details.SchoolName,
			LocalAuthority = detailsToUpdate.LocalAuthority ?? existingProject.Details.LocalAuthority,
			ApplicationReferenceNumber = detailsToUpdate.ApplicationReferenceNumber ?? existingProject.Details.ApplicationReferenceNumber,
			ProjectStatus = detailsToUpdate.ProjectStatus ?? existingProject.Details.ProjectStatus,
			ApplicationReceivedDate = detailsToUpdate.ApplicationReceivedDate ?? existingProject.Details.ApplicationReceivedDate,
			AssignedDate = detailsToUpdate.AssignedDate ?? existingProject.Details.AssignedDate,
			HeadTeacherBoardDate = detailsToUpdate.HeadTeacherBoardDate ?? existingProject.Details.HeadTeacherBoardDate,
			OpeningDate = detailsToUpdate.OpeningDate ?? existingProject.Details.OpeningDate,
			BaselineDate = detailsToUpdate.BaselineDate ?? existingProject.Details.BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate ?? existingProject.Details.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate ?? existingProject.Details.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments ?? existingProject.Details.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink ?? existingProject.Details.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete ?? existingProject.Details.LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = detailsToUpdate.RecommendationForProject ?? existingProject.Details.RecommendationForProject,
			Author = detailsToUpdate.Author ?? existingProject.Details.Author,
			Version = detailsToUpdate.Version ?? existingProject.Details.Version,
			ClearedBy = detailsToUpdate.ClearedBy ?? existingProject.Details.ClearedBy,
			AcademyOrderRequired = detailsToUpdate.AcademyOrderRequired ?? existingProject.Details.AcademyOrderRequired,
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion ?? existingProject.Details.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate ?? existingProject.Details.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = detailsToUpdate.PreviousHeadTeacherBoardLink ?? existingProject.Details.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = detailsToUpdate.TrustReferenceNumber ?? existingProject.Details.TrustReferenceNumber,
			NameOfTrust = detailsToUpdate.NameOfTrust ?? existingProject.Details.NameOfTrust,
			SponsorReferenceNumber = detailsToUpdate.SponsorReferenceNumber ?? existingProject.Details.SponsorReferenceNumber,
			SponsorName = detailsToUpdate.SponsorName ?? existingProject.Details.SponsorName,
			AcademyTypeAndRoute = detailsToUpdate.AcademyTypeAndRoute ?? existingProject.Details.AcademyTypeAndRoute,
			Form7Received = detailsToUpdate.Form7Received ?? existingProject.Details.Form7Received,
			Form7ReceivedDate = detailsToUpdate.Form7ReceivedDate ?? existingProject.Details.Form7ReceivedDate,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate ?? existingProject.Details.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete ?? existingProject.Details.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount ?? existingProject.Details.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason ?? existingProject.Details.ConversionSupportGrantChangeReason,
			Region = detailsToUpdate.Region ?? existingProject.Details.Region,

			// general info
			SchoolPhase = detailsToUpdate.SchoolPhase ?? existingProject.Details.SchoolPhase,
			AgeRange = detailsToUpdate.AgeRange ?? existingProject.Details.AgeRange,
			SchoolType = detailsToUpdate.SchoolType ?? existingProject.Details.SchoolType,
			ActualPupilNumbers = detailsToUpdate.ActualPupilNumbers ?? existingProject.Details.ActualPupilNumbers,
			Capacity = detailsToUpdate.Capacity ?? existingProject.Details.Capacity,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber ?? existingProject.Details.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = detailsToUpdate.PercentageFreeSchoolMeals ?? existingProject.Details.PercentageFreeSchoolMeals,
			PartOfPfiScheme = detailsToUpdate.PartOfPfiScheme ?? existingProject.Details.PartOfPfiScheme,
			ViabilityIssues = detailsToUpdate.ViabilityIssues ?? existingProject.Details.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit ?? existingProject.Details.FinancialDeficit,
			DiocesanTrust = detailsToUpdate.DiocesanTrust ?? existingProject.Details.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = detailsToUpdate.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust ?? existingProject.Details.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters ?? existingProject.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation ?? existingProject.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = detailsToUpdate.MemberOfParliamentParty ?? existingProject.Details.MemberOfParliamentParty,
			MemberOfParliamentName = detailsToUpdate.MemberOfParliamentName ?? existingProject.Details.MemberOfParliamentName,

			GeneralInformationSectionComplete = detailsToUpdate.GeneralInformationSectionComplete ?? existingProject.Details.GeneralInformationSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation ?? existingProject.Details.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = detailsToUpdate.RationaleForProject ?? existingProject.Details.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust ?? existingProject.Details.RationaleForTrust,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete ?? existingProject.Details.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = detailsToUpdate.RisksAndIssues ?? existingProject.Details.RisksAndIssues,
			EqualitiesImpactAssessmentConsidered = detailsToUpdate.EqualitiesImpactAssessmentConsidered ?? existingProject.Details.EqualitiesImpactAssessmentConsidered,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete ?? existingProject.Details.RisksAndIssuesSectionComplete,

			// legal requirements
			Consultation = detailsToUpdate.Consultation ?? existingProject.Details.Consultation,
			GoverningBodyResolution = detailsToUpdate.GoverningBodyResolution ?? existingProject.Details.GoverningBodyResolution,
			DiocesanConsent = detailsToUpdate.DiocesanConsent ?? existingProject.Details.DiocesanConsent,
			FoundationConsent = detailsToUpdate.FoundationConsent ?? existingProject.Details.FoundationConsent,
			LegalRequirementsSectionComplete = detailsToUpdate.LegalRequirementsSectionComplete ?? existingProject.Details.LegalRequirementsSectionComplete,

			// school budget info
			EndOfCurrentFinancialYear = detailsToUpdate.EndOfCurrentFinancialYear ?? existingProject.Details.EndOfCurrentFinancialYear,
			EndOfNextFinancialYear = detailsToUpdate.EndOfNextFinancialYear ?? existingProject.Details.EndOfNextFinancialYear,
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear ?? existingProject.Details.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear ?? existingProject.Details.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear ?? existingProject.Details.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear ?? existingProject.Details.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation ?? existingProject.Details.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete ?? existingProject.Details.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			YearOneProjectedCapacity = detailsToUpdate.YearOneProjectedCapacity ?? existingProject.Details.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = detailsToUpdate.YearOneProjectedPupilNumbers ?? existingProject.Details.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = detailsToUpdate.YearTwoProjectedCapacity ?? existingProject.Details.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = detailsToUpdate.YearTwoProjectedPupilNumbers ?? existingProject.Details.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = detailsToUpdate.YearThreeProjectedCapacity ?? existingProject.Details.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = detailsToUpdate.YearThreeProjectedPupilNumbers ?? existingProject.Details.YearThreeProjectedPupilNumbers,
			SchoolPupilForecastsAdditionalInformation = detailsToUpdate.SchoolPupilForecastsAdditionalInformation ?? existingProject.Details.SchoolPupilForecastsAdditionalInformation,

			// key stage performance tables
			KeyStage2PerformanceAdditionalInformation = detailsToUpdate.KeyStage2PerformanceAdditionalInformation ?? existingProject.Details.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = detailsToUpdate.KeyStage4PerformanceAdditionalInformation ?? existingProject.Details.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = detailsToUpdate.KeyStage5PerformanceAdditionalInformation ?? existingProject.Details.KeyStage5PerformanceAdditionalInformation,

			// assigned user
			AssignedUser = detailsToUpdate.AssignedUser != null
				? MapServiceUser(detailsToUpdate.AssignedUser)
				: MapDomainUser(existingProject.Details.AssignedUser)
		};
	}

	private static DomainUser? MapServiceUser(ServiceUser? user)
	{
		switch (user)
		{
			case not null:
				return new DomainUser(user.Id, user.FullName, user.EmailAddress);
			default:
				return null;
		}
	}
	private static DomainUser? MapDomainUser(DomainUser? user)
	{
		switch (user)
		{
			case not null:
				return new DomainUser(user.Id, user.FullName, user.EmailAddress);
			default:
				return null;
		}
	}
}
