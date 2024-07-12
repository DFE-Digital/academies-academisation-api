using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using User = Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate.User;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectServiceModelMapper
{
	internal static ConversionProjectServiceModel MapToServiceModel(this IProject project)
	{
		ConversionProjectServiceModel serviceModel = new(project.Id, project.Details.Urn)
		{
			FormAMatProjectId = project.FormAMatProjectId,
			SchoolSharePointId = project.SchoolSharePointId,
			ApplicationSharePointId = project.ApplicationSharePointId,
			IsFormAMat = project.Details.IsFormAMat,
			IfdPipelineId = project.Details.IfdPipelineId,
			SchoolName = project.Details.SchoolName,
			CreatedOn = project.CreatedOn,
			LocalAuthority = project.Details.LocalAuthority,
			ApplicationReferenceNumber = project.Details.ApplicationReferenceNumber,
			ProjectStatus = project.Details.ProjectStatus,
			ApplicationReceivedDate = project.Details.ApplicationReceivedDate,
			AssignedDate = project.Details.AssignedDate,
			HeadTeacherBoardDate = project.Details.HeadTeacherBoardDate,
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
			DaoPackSentDate = project.Details.DaoPackSentDate,
			PreviousHeadTeacherBoardDateQuestion = project.Details.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = project.Details.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = project.Details.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = project.Details.TrustReferenceNumber,
			NameOfTrust = project.Details.NameOfTrust,
			SponsorReferenceNumber = project.Details.SponsorReferenceNumber,
			SponsorName = project.Details.SponsorName,
			AcademyTypeAndRoute = project.Details.AcademyTypeAndRoute,
			Form7Received = project.Details.Form7Received,
			Form7ReceivedDate = project.Details.Form7ReceivedDate,
			ProposedConversionDate = project.Details.ProposedConversionDate,
			SchoolAndTrustInformationSectionComplete = project.Details.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = project.Details.ConversionSupportGrantAmount,  // had to make this nullable or move it to the top
			ConversionSupportGrantChangeReason = project.Details.ConversionSupportGrantChangeReason,
			ConversionSupportGrantType = project.Details.ConversionSupportGrantType,
			ConversionSupportGrantEnvironmentalImprovementGrant = project.Details.ConversionSupportGrantEnvironmentalImprovementGrant,
			ConversionSupportGrantAmountChanged = project.Details.ConversionSupportGrantAmountChanged,
			ConversionSupportGrantNumberOfSites = project.Details.ConversionSupportGrantNumberOfSites,
			Region = project.Details.Region,

			// School Overview
			SchoolPhase = project.Details.SchoolPhase,
			AgeRange = project.Details.AgeRange,
			SchoolType = project.Details.SchoolType,
			ActualPupilNumbers = project.Details.ActualPupilNumbers,
			Capacity = project.Details.Capacity,
			PublishedAdmissionNumber = project.Details.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = project.Details.PercentageFreeSchoolMeals,
			PartOfPfiScheme = project.Details.PartOfPfiScheme,
			PfiSchemeDetails = project.Details.PfiSchemeDetails,
			ViabilityIssues = project.Details.ViabilityIssues,
			NumberOfPlacesFundedFor = project.Details.NumberOfPlacesFundedFor,
			NumberOfResidentialPlaces = project.Details.NumberOfResidentialPlaces,
			NumberOfFundedResidentialPlaces = project.Details.NumberOfFundedResidentialPlaces,
			FinancialDeficit = project.Details.FinancialDeficit,
			DiocesanTrust = project.Details.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = project.Details.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = project.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentNameAndParty = project.Details.MemberOfParliamentNameAndParty,
			SchoolOverviewSectionComplete = project.Details.SchoolOverviewSectionComplete,
			PupilsAttendingGroupPermanentlyExcluded = project.Details.PupilsAttendingGroupPermanentlyExcluded,
			PupilsAttendingGroupMedicalAndHealthNeeds = project.Details.PupilsAttendingGroupMedicalAndHealthNeeds,
			PupilsAttendingGroupTeenageMums = project.Details.PupilsAttendingGroupTeenageMums,
			NumberOfAlternativeProvisionPlaces = project.Details.NumberOfAlternativeProvisionPlaces,
			NumberOfMedicalPlaces = project.Details.NumberOfMedicalPlaces,
			NumberOfSENUnitPlaces = project.Details.NumberOfSENUnitPlaces,
			NumberOfPost16Places = project.Details.NumberOfPost16Places,

			// Annex B
			AnnexBFormReceived = project.Details.AnnexBFormReceived,
			AnnexBFormUrl = project.Details.AnnexBFormUrl,

			//External Application Form
			ExternalApplicationFormSaved = project.Details.ExternalApplicationFormSaved,
			ExternalApplicationFormUrl = project.Details.ExternalApplicationFormUrl,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = project.Details.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = project.Details.RationaleForProject,
			RationaleForTrust = project.Details.RationaleForTrust,
			RationaleSectionComplete = project.Details.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = project.Details.RisksAndIssues,
			RisksAndIssuesSectionComplete = project.Details.RisksAndIssuesSectionComplete,

			// legal requirements
			Consultation = project.Details.Consultation,
			GoverningBodyResolution = project.Details.GoverningBodyResolution,
			DiocesanConsent = project.Details.DiocesanConsent,
			FoundationConsent = project.Details.FoundationConsent,
			LegalRequirementsSectionComplete = project.Details.LegalRequirementsSectionComplete,

			// school budget info
			EndOfCurrentFinancialYear = project.Details.EndOfCurrentFinancialYear,
			EndOfNextFinancialYear = project.Details.EndOfNextFinancialYear,
			RevenueCarryForwardAtEndMarchCurrentYear = project.Details.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = project.Details.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = project.Details.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = project.Details.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = project.Details.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = project.Details.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
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
			EducationalAttendanceAdditionalInformation = project.Details.EducationalAttendanceAdditionalInformation,

			AssignedUser = project.Details.AssignedUser?.Id == null
				? null
				: new User(project.Details.AssignedUser!.Id, project.Details.AssignedUser.FullName, project.Details.AssignedUser.EmailAddress),

			Notes = project.Notes?.ToProjectNoteServiceModels().ToList(),

			ProjectDatesSectionComplete = project.Details.ProjectDatesSectionComplete,
		};

		return serviceModel;
	}

	internal static FormAMatProjectServiceModel MapToFormAMatServiceModel(this IFormAMatProject formAMatProject, IEnumerable<IProject> projects)
	{
		FormAMatProjectServiceModel serviceModel = new(formAMatProject.Id, formAMatProject.ProposedTrustName, formAMatProject.ApplicationReference, new User(formAMatProject.AssignedUser?.Id ?? Guid.Empty, formAMatProject.AssignedUser?.FullName ?? string.Empty, formAMatProject.AssignedUser?.EmailAddress ?? string.Empty), formAMatProject?.ReferenceNumber ?? string.Empty)
		{
			projects = projects.Where(x => x.FormAMatProjectId == formAMatProject.Id).Select(p => p.MapToServiceModel()).ToList()
		};
		return serviceModel;
	}
	private static IEnumerable<ConversionProjectDeleteNote> ToProjectNoteServiceModels(this IEnumerable<IProjectNote>? notes)
	{
		if (notes is null) return Enumerable.Empty<ConversionProjectDeleteNote>();

		return notes.Select(note => new ConversionProjectDeleteNote
		{
			Author = note.Author,
			Note = note.Note,
			Subject = note.Subject,
			Date = note.Date
		});
	}
}
