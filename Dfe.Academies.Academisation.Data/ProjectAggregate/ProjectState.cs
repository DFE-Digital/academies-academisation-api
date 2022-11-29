﻿using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

[Table(name: "Project")]
public class ProjectState : BaseEntity
{
	public int Urn { get; set; }
	public int? IfdPipelineId { get; set; }
	public string? SchoolName { get; set; }
	public string? ApplicationReferenceNumber { get; set; }
	public string? ProjectStatus { get; set; }
	public DateTime? ApplicationReceivedDate { get; set; }
	public DateTime? AssignedDate { get; set; }
	public DateTime? HeadTeacherBoardDate { get; set; }
	public DateTime? OpeningDate { get; set; }
	public DateTime? BaselineDate { get; set; }

	//la summary page
	public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
	public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
	public string? LocalAuthorityInformationTemplateComments { get; set; }
	public string? LocalAuthorityInformationTemplateLink { get; set; }
	public bool? LocalAuthorityInformationTemplateSectionComplete { get; set; }

	//school/trust info
	public string? RecommendationForProject { get; set; }
	public string? Author { get; set; }
	public string? Version { get; set; }
	public string? ClearedBy { get; set; }
	public string? AcademyOrderRequired { get; set; }
	public string? PreviousHeadTeacherBoardDateQuestion { get; set; }
	public DateTime? PreviousHeadTeacherBoardDate { get; set; }
	public string? PreviousHeadTeacherBoardLink { get; set; }
	public string? TrustReferenceNumber { get; set; }
	public string? NameOfTrust { get; set; }
	public string? SponsorReferenceNumber { get; set; }
	public string? SponsorName { get; set; }
	public string? AcademyTypeAndRoute { get; set; }
	public DateTime? ProposedAcademyOpeningDate { get; set; }
	public bool? SchoolAndTrustInformationSectionComplete { get; set; }
	public decimal? ConversionSupportGrantAmount { get; set; }
	public string? ConversionSupportGrantChangeReason { get; set; }

	//general info
	public string? SchoolPhase { get; set; }
	public string? AgeRange { get; set; }
	public string? SchoolType { get; set; }
	public int? ActualPupilNumbers { get; set; }
	public int? Capacity { get; set; }
	public string? PublishedAdmissionNumber { get; set; }
	public decimal? PercentageFreeSchoolMeals { get; set; }
	public string? PartOfPfiScheme { get; set; }
	public string? ViabilityIssues { get; set; }
	public string? FinancialDeficit { get; set; }
	public string? DiocesanTrust { get; set; }
	public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
	public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
	public string? DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
	public string? MemberOfParliamentParty { get; set; }
	public string? MemberOfParliamentName { get; set; }

	public bool? GeneralInformationSectionComplete { get; set; }

	//school performance ofsted information
	public string? SchoolPerformanceAdditionalInformation { get; set; }

	// rationale
	public string? RationaleForProject { get; set; }
	public string? RationaleForTrust { get; set; }
	public bool? RationaleSectionComplete { get; set; }

	// risk and issues
	public string? RisksAndIssues { get; set; }
	public string? EqualitiesImpactAssessmentConsidered { get; set; }
	public bool? RisksAndIssuesSectionComplete { get; set; }

	// legal requirements
	public YesNoNotApplicable? GoverningBodyResolution { get; set; }
	public YesNoNotApplicable? Consultation { get; set; }
	public YesNoNotApplicable? DiocesanConsent { get; set; }
	public YesNoNotApplicable? FoundationConsent { get; set; }
	public bool? LegalRequirementsSectionComplete { get; set; }

	// school budget info
	public DateTime? EndOfCurrentFinancialYear { get; set; }
	public DateTime? EndOfNextFinancialYear { get; set; }
	public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
	public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
	public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
	public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
	public string? SchoolBudgetInformationAdditionalInformation { get; set; }
	public bool? SchoolBudgetInformationSectionComplete { get; set; }

	// pupil schools forecast
	public int? YearOneProjectedCapacity { get; set; }
	public int? YearOneProjectedPupilNumbers { get; set; }
	public int? YearTwoProjectedCapacity { get; set; }
	public int? YearTwoProjectedPupilNumbers { get; set; }
	public int? YearThreeProjectedCapacity { get; set; }
	public int? YearThreeProjectedPupilNumbers { get; set; }
	public string? SchoolPupilForecastsAdditionalInformation { get; set; }

	// key stage performance tables
	public string? KeyStage2PerformanceAdditionalInformation { get; set; }
	public string? KeyStage4PerformanceAdditionalInformation { get; set; }
	public string? KeyStage5PerformanceAdditionalInformation { get; set; }

	// assigned user
	public Guid? AssignedUserId { get; set; }
	public string? AssignedUserEmailAddress { get; set; }
	public string? AssignedUserFullName { get; set; }

	internal Project MapToDomain()
	{
		ProjectDetails projectDetails = new(Urn)
		{
			IfdPipelineId = IfdPipelineId,
			SchoolName = SchoolName,
			ApplicationReferenceNumber = ApplicationReferenceNumber,
			ProjectStatus = ProjectStatus,
			ApplicationReceivedDate = ApplicationReceivedDate,
			AssignedDate = AssignedDate,
			HeadTeacherBoardDate = HeadTeacherBoardDate,
			OpeningDate = OpeningDate,
			BaselineDate = BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = RecommendationForProject,
			Author = Author,
			Version = Version,
			ClearedBy = ClearedBy,
			AcademyOrderRequired = AcademyOrderRequired,
			PreviousHeadTeacherBoardDateQuestion = PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = TrustReferenceNumber,
			NameOfTrust = NameOfTrust,
			SponsorReferenceNumber = SponsorReferenceNumber,
			SponsorName = SponsorName,
			AcademyTypeAndRoute = AcademyTypeAndRoute,
			ProposedAcademyOpeningDate = ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = ConversionSupportGrantChangeReason,

			// general info
			SchoolPhase = SchoolPhase,
			AgeRange = AgeRange,
			SchoolType = SchoolType,
			ActualPupilNumbers = ActualPupilNumbers,
			Capacity = Capacity,
			PublishedAdmissionNumber = PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = PercentageFreeSchoolMeals,
			PartOfPfiScheme = PartOfPfiScheme,
			ViabilityIssues = ViabilityIssues,
			FinancialDeficit = FinancialDeficit,
			DiocesanTrust = DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = MemberOfParliamentParty,
			MemberOfParliamentName = MemberOfParliamentName,

			GeneralInformationSectionComplete = GeneralInformationSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = RationaleForProject,
			RationaleForTrust = RationaleForTrust,
			RationaleSectionComplete = RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = RisksAndIssues,
			EqualitiesImpactAssessmentConsidered = EqualitiesImpactAssessmentConsidered,
			RisksAndIssuesSectionComplete = RisksAndIssuesSectionComplete,

			// legal requirements
			GoverningBodyResolution = GoverningBodyResolution,
			Consultation = Consultation,
			DiocesanConsent = DiocesanConsent,
			FoundationConsent = FoundationConsent,
			LegalRequirementsSectionComplete = LegalRequirementsSectionComplete,

			// school budget info
			EndOfCurrentFinancialYear = EndOfCurrentFinancialYear,
			EndOfNextFinancialYear = EndOfNextFinancialYear,
			RevenueCarryForwardAtEndMarchCurrentYear = RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			YearOneProjectedCapacity = YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = YearThreeProjectedPupilNumbers,
			SchoolPupilForecastsAdditionalInformation = SchoolPupilForecastsAdditionalInformation,

			// key stage performance tables
			KeyStage2PerformanceAdditionalInformation = KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = KeyStage5PerformanceAdditionalInformation,

			// assigned user
			AssignedUser = AssignedUserId == null
				? null
				: new User(AssignedUserId.Value, AssignedUserFullName ?? "", AssignedUserEmailAddress ?? "")
		};

		return new Project(Id, projectDetails);
	}

	internal static ProjectState MapFromDomain(IProject project)
	{
		return new ProjectState
		{
			Id = project.Id,
			Urn = project.Details.Urn,
			IfdPipelineId = project.Details.IfdPipelineId,
			SchoolName = project.Details.SchoolName,
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
			ConversionSupportGrantAmount = project.Details.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = project.Details.ConversionSupportGrantChangeReason,

			// general info
			SchoolPhase = project.Details.SchoolPhase,
			AgeRange = project.Details.AgeRange,
			SchoolType = project.Details.SchoolType,
			ActualPupilNumbers = project.Details.ActualPupilNumbers,
			Capacity = project.Details.Capacity,
			PublishedAdmissionNumber = project.Details.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = project.Details.PercentageFreeSchoolMeals,
			PartOfPfiScheme = project.Details.PartOfPfiScheme,
			ViabilityIssues = project.Details.ViabilityIssues,
			FinancialDeficit = project.Details.FinancialDeficit,
			DiocesanTrust = project.Details.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = project.Details.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
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

			// legal requirements
			GoverningBodyResolution = project.Details.GoverningBodyResolution,
			Consultation = project.Details.Consultation,
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

			// assigned user
			AssignedUserId = project.Details.AssignedUser?.Id,
			AssignedUserFullName = project.Details.AssignedUser?.FullName,
			AssignedUserEmailAddress = project.Details.AssignedUser?.EmailAddress
		};
	}
}
