using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using User = Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate.User;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

internal static class LegacyProjectServiceModelMapper
{
	internal static LegacyProjectServiceModel MapToServiceModel(this IProject project)
	{
		LegacyProjectServiceModel serviceModel = new(project.Id, project.Details.Urn)
		{
			IfdPipelineId = project.Details.IfdPipelineId,
			SchoolName = project.Details.SchoolName,
			CreatedOn = project.Details.CreatedOn,
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
			ProposedAcademyOpeningDate = project.Details.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = project.Details.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = project.Details.ConversionSupportGrantAmount,  // had to make this nullable or move it to the top
			ConversionSupportGrantChangeReason = project.Details.ConversionSupportGrantChangeReason,
			Region = project.Details.Region,

			// general info
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
			FinancialDeficit = project.Details.FinancialDeficit,
			DiocesanTrust = project.Details.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = project.Details.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = project.Details.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = project.Details.MemberOfParliamentParty,
			MemberOfParliamentName = project.Details.MemberOfParliamentName,
			GeneralInformationSectionComplete = project.Details.GeneralInformationSectionComplete,

			// Annex B
			AnnexBFormReceived = project.Details.AnnexBFormReceived,
			AnnexBFormUrl = project.Details.AnnexBFormUrl,

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

			AssignedUser = project.Details.AssignedUser?.Id == null
				? null
				: new User(project.Details.AssignedUser!.Id, project.Details.AssignedUser.FullName, project.Details.AssignedUser.EmailAddress),

			Notes = project.Details.Notes.ToProjectNoteServiceModels().ToList()
		};

		return serviceModel;
	}

	private static IEnumerable<ProjectNoteServiceModel> ToProjectNoteServiceModels(this IEnumerable<ProjectNote>? notes)
	{
		if (notes is null) return Enumerable.Empty<ProjectNoteServiceModel>();

		return notes.Select(note => new ProjectNoteServiceModel
		{
			Author = note.Author, Note = note.Note, Subject = note.Subject, Date = note.Date
		});
	}
}
