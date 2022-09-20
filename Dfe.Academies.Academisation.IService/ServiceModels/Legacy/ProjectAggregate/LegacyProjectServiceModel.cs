namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

public record LegacyProjectServiceModel(
	int Id,
	int? Urn = null,
	string? SchoolName = null,
	string? LocalAuthority = null,
	string? ApplicationReferenceNumber = null,
	string? ProjectStatus = null,
	DateTime? ApplicationReceivedDate = null,
	DateTime? AssignedDate = null,
	DateTime? HeadTeacherBoardDate = null,
	DateTime? OpeningDate = null,
	DateTime? BaselineDate = null,

	// la summary page
	DateTime? LocalAuthorityInformationTemplateSentDate = null,
	DateTime? LocalAuthorityInformationTemplateReturnedDate = null,
	string? LocalAuthorityInformationTemplateComments = null,
	string? LocalAuthorityInformationTemplateLink = null,
	bool? LocalAuthorityInformationTemplateSectionComplete = null,

	// school/trust info
	string? RecommendationForProject = null,
	string? Author = null,
	string? Version = null,
	string? ClearedBy = null,
	string? AcademyOrderRequired = null,
	string? PreviousHeadTeacherBoardDateQuestion = null,
	DateTime? PreviousHeadTeacherBoardDate = null,
	string? PreviousHeadTeacherBoardLink = null,
	string? TrustReferenceNumber = null,
	string? NameOfTrust = null,
	string? SponsorReferenceNumber = null,
	string? SponsorName = null,
	string? AcademyTypeAndRoute = null,
	DateTime? ProposedAcademyOpeningDate = null,
	bool? SchoolAndTrustInformationSectionComplete = null,
	decimal? ConversionSupportGrantAmount = null,  // had to make this nullable or move it to the top
	string? ConversionSupportGrantChangeReason = null,

	// general info
	string? PublishedAdmissionNumber = null,
	string? PartOfPfiScheme = null,
	string? ViabilityIssues = null,
	string? FinancialDeficit = null,
	decimal? DistanceFromSchoolToTrustHeadquarters = null,
	string? DistanceFromSchoolToTrustHeadquartersAdditionalInformation = null,
	string? MemberOfParliamentParty = null,
	string? MemberOfParliamentName = null,

	bool? GeneralInformationSectionComplete = null,

	// school performance ofsted information
	string? SchoolPerformanceAdditionalInformation = null,

	// rationale
	string? RationaleForProject = null,
	string? RationaleForTrust = null,
	bool? RationaleSectionComplete = null,

	// risk and issues
	string? RisksAndIssues = null,
	string? EqualitiesImpactAssessmentConsidered = null,
	bool? RisksAndIssuesSectionComplete = null,

	// school budget info
	decimal? RevenueCarryForwardAtEndMarchCurrentYear = null,
	decimal? ProjectedRevenueBalanceAtEndMarchNextYear = null,
	decimal? CapitalCarryForwardAtEndMarchCurrentYear = null,
	decimal? CapitalCarryForwardAtEndMarchNextYear = null,
	string? SchoolBudgetInformationAdditionalInformation = null,
	bool? SchoolBudgetInformationSectionComplete = null,

	// pupil schools forecast
	//int? CurrentYearCapacity = null,
	//int? CurrentYearPupilNumbers = null,
	int? YearOneProjectedCapacity = null,
	int? YearOneProjectedPupilNumbers = null,
	int? YearTwoProjectedCapacity = null,
	int? YearTwoProjectedPupilNumbers = null,
	int? YearThreeProjectedCapacity = null,
	int? YearThreeProjectedPupilNumbers = null,
	string? SchoolPupilForecastsAdditionalInformation = null,

	// key stage performance tables
	string? KeyStage2PerformanceAdditionalInformation = null,
	string? KeyStage4PerformanceAdditionalInformation = null,
	string? KeyStage5PerformanceAdditionalInformation = null,
	string? Upin = null,
	string? NewAcademyUrn = null
);
