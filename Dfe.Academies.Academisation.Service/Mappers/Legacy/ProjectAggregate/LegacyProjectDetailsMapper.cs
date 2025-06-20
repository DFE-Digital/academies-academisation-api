﻿using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using DomainUser = Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.User;
using ServiceUser = Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate.User;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectDetailsMapper
{
	internal static ProjectDetails MapNonEmptyFields(this ConversionProjectServiceModel detailsToUpdate, IProject existingProject)
	{
		// this needs to be done for all dates
		// detailsToUpdate.HeadTeacherBoardDate.Equals(default(DateTime)) ? null : detailsToUpdate.HeadTeacherBoardDate ?? existingProject.Details.HeadTeacherBoardDate,
		// otherwise they wouldn't ever be able to be set back to null
		// not the best solution but without rewriting the update funtionality this is the only option
		var details = new ProjectDetails
		{
			Urn = detailsToUpdate.Urn ?? existingProject.Details.Urn,
			TrustUkprn = detailsToUpdate.TrustUkprn ?? existingProject.Details.TrustUkprn,
			IfdPipelineId = detailsToUpdate.IfdPipelineId ?? existingProject.Details.IfdPipelineId,
			SchoolName = detailsToUpdate.SchoolName ?? existingProject.Details.SchoolName,
			LocalAuthority = detailsToUpdate.LocalAuthority ?? existingProject.Details.LocalAuthority,
			ApplicationReferenceNumber = detailsToUpdate.ApplicationReferenceNumber ?? existingProject.Details.ApplicationReferenceNumber,
			ProjectStatus = detailsToUpdate.ProjectStatus ?? existingProject.Details.ProjectStatus,
			ApplicationReceivedDate = detailsToUpdate.ApplicationReceivedDate ?? existingProject.Details.ApplicationReceivedDate,
			AssignedDate = detailsToUpdate.AssignedDate ?? existingProject.Details.AssignedDate,
			HeadTeacherBoardDate = detailsToUpdate.HeadTeacherBoardDate.Equals(default(DateTime)) ? null : detailsToUpdate.HeadTeacherBoardDate ?? existingProject.Details.HeadTeacherBoardDate,
			BaselineDate = detailsToUpdate.BaselineDate ?? existingProject.Details.BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate.Equals(default(DateTime)) ? null : detailsToUpdate.LocalAuthorityInformationTemplateSentDate ?? existingProject.Details.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate.Equals(default(DateTime)) ? null : detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate ?? existingProject.Details.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments ?? existingProject.Details.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink ?? existingProject.Details.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete ?? existingProject.Details.LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = detailsToUpdate.RecommendationForProject ?? existingProject.Details.RecommendationForProject,
			Author = detailsToUpdate.Author ?? existingProject.Details.Author,
			Version = detailsToUpdate.Version ?? existingProject.Details.Version,
			ClearedBy = detailsToUpdate.ClearedBy ?? existingProject.Details.ClearedBy,
			DaoPackSentDate = detailsToUpdate.DaoPackSentDate ?? existingProject.Details.DaoPackSentDate,
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
			ProposedConversionDate = existingProject.Details.ProposedConversionDate,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete ?? existingProject.Details.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount ?? existingProject.Details.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason ?? existingProject.Details.ConversionSupportGrantChangeReason,
			ConversionSupportGrantType = detailsToUpdate.ConversionSupportGrantType ?? existingProject.Details.ConversionSupportGrantType,
			ConversionSupportGrantEnvironmentalImprovementGrant = detailsToUpdate.ConversionSupportGrantEnvironmentalImprovementGrant ?? existingProject.Details.ConversionSupportGrantEnvironmentalImprovementGrant,
			ConversionSupportGrantAmountChanged = detailsToUpdate.ConversionSupportGrantAmountChanged ?? existingProject.Details.ConversionSupportGrantAmountChanged,
			ConversionSupportGrantNumberOfSites = detailsToUpdate.ConversionSupportGrantNumberOfSites ?? existingProject.Details.ConversionSupportGrantNumberOfSites,
			Region = detailsToUpdate.Region ?? existingProject.Details.Region,

			// Annex B
			AnnexBFormReceived = detailsToUpdate.AnnexBFormReceived ?? existingProject.Details.AnnexBFormReceived,
			AnnexBFormUrl = detailsToUpdate.AnnexBFormReceived is true ? detailsToUpdate.AnnexBFormUrl : string.Empty,

			// External Application Form
			ExternalApplicationFormSaved = detailsToUpdate.ExternalApplicationFormSaved ?? existingProject.Details.ExternalApplicationFormSaved,
			ExternalApplicationFormUrl = detailsToUpdate.ExternalApplicationFormSaved is true ? detailsToUpdate.ExternalApplicationFormUrl : string.Empty,

			// School Overview
			SchoolPhase = detailsToUpdate.SchoolPhase ?? existingProject.Details.SchoolPhase,
			AgeRange = detailsToUpdate.AgeRange ?? existingProject.Details.AgeRange,
			SchoolType = detailsToUpdate.SchoolType ?? existingProject.Details.SchoolType,
			ActualPupilNumbers = detailsToUpdate.ActualPupilNumbers ?? existingProject.Details.ActualPupilNumbers,
			Capacity = detailsToUpdate.Capacity ?? existingProject.Details.Capacity,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber ?? existingProject.Details.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = detailsToUpdate.PercentageFreeSchoolMeals ?? existingProject.Details.PercentageFreeSchoolMeals,
			NumberOfPlacesFundedFor = detailsToUpdate.NumberOfPlacesFundedFor ?? existingProject.Details.NumberOfPlacesFundedFor,
			NumberOfResidentialPlaces = detailsToUpdate.NumberOfResidentialPlaces ?? existingProject.Details.NumberOfResidentialPlaces,
			NumberOfFundedResidentialPlaces = detailsToUpdate.NumberOfFundedResidentialPlaces ?? existingProject.Details.NumberOfFundedResidentialPlaces,
			PartOfPfiScheme = detailsToUpdate.PartOfPfiScheme ?? existingProject.Details.PartOfPfiScheme,
			PfiSchemeDetails = detailsToUpdate.PfiSchemeDetails ?? existingProject.Details.PfiSchemeDetails,
			ViabilityIssues = detailsToUpdate.ViabilityIssues ?? existingProject.Details.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit ?? existingProject.Details.FinancialDeficit,
			DiocesanTrust = detailsToUpdate.DiocesanTrust ?? existingProject.Details.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = detailsToUpdate.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust ?? existingProject.Details.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters ?? existingProject.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation ?? existingProject.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentNameAndParty = detailsToUpdate.MemberOfParliamentNameAndParty ?? existingProject.Details.MemberOfParliamentNameAndParty,
			NumberOfAlternativeProvisionPlaces = detailsToUpdate.NumberOfAlternativeProvisionPlaces ?? existingProject.Details.NumberOfAlternativeProvisionPlaces,
			NumberOfMedicalPlaces = detailsToUpdate.NumberOfMedicalPlaces ?? existingProject.Details.NumberOfMedicalPlaces,
			NumberOfPost16Places = detailsToUpdate.NumberOfPost16Places ?? existingProject.Details.NumberOfPost16Places,
			NumberOfSENUnitPlaces = detailsToUpdate.NumberOfSENUnitPlaces ?? existingProject.Details.NumberOfSENUnitPlaces,
			PupilsAttendingGroupMedicalAndHealthNeeds = detailsToUpdate.PupilsAttendingGroupMedicalAndHealthNeeds ?? existingProject.Details.PupilsAttendingGroupMedicalAndHealthNeeds,
			PupilsAttendingGroupPermanentlyExcluded = detailsToUpdate.PupilsAttendingGroupPermanentlyExcluded ?? existingProject.Details.PupilsAttendingGroupPermanentlyExcluded,
			PupilsAttendingGroupTeenageMums = detailsToUpdate.PupilsAttendingGroupTeenageMums ?? existingProject.Details.PupilsAttendingGroupTeenageMums,

			SchoolOverviewSectionComplete = detailsToUpdate.SchoolOverviewSectionComplete ?? existingProject.Details.SchoolOverviewSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation ?? existingProject.Details.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = detailsToUpdate.RationaleForProject ?? existingProject.Details.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust ?? existingProject.Details.RationaleForTrust,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete ?? existingProject.Details.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = detailsToUpdate.RisksAndIssues ?? existingProject.Details.RisksAndIssues,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete ?? existingProject.Details.RisksAndIssuesSectionComplete,

			// Public sector equality duty
			PublicEqualityDutyImpact = existingProject.Details.PublicEqualityDutyImpact,
			PublicEqualityDutyReduceImpactReason = existingProject.Details.PublicEqualityDutyReduceImpactReason,
			PublicEqualityDutySectionComplete = existingProject.Details.PublicEqualityDutySectionComplete,

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

			// assigned user
			AssignedUser = detailsToUpdate.AssignedUser != null
				? MapServiceUser(detailsToUpdate.AssignedUser)
				: MapDomainUser(existingProject.Details.AssignedUser)
		};

		// fix for maintaining data in patch that is moved out to indivisual update
		details.SetPerformanceData(existingProject.Details.KeyStage2PerformanceAdditionalInformation, existingProject.Details.KeyStage4PerformanceAdditionalInformation, existingProject.Details.KeyStage5PerformanceAdditionalInformation, existingProject.Details.EducationalAttendanceAdditionalInformation);
		details.SetIsFormAMat(existingProject.Details.IsFormAMat);
		return details;
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
