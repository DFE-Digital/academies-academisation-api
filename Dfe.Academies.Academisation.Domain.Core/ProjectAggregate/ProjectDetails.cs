namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

/// <summary>
///		Represents the project data
/// </summary>
/// <remarks>
///     The notes associated with the project do not affect equality, while all other properties do.
/// </remarks>
public class ProjectDetails : IEquatable<ProjectDetails>
{
	public ProjectDetails(int Urn,
						  int? IfdPipelineId = null,
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
						  decimal? ConversionSupportGrantAmount = null,
						  string? ConversionSupportGrantChangeReason = null,
						  string? Region = null,

						  // general info
						  string? SchoolPhase = null,
						  string? AgeRange = null,
						  string? SchoolType = null,
						  int? ActualPupilNumbers = null,
						  int? Capacity = null,
						  string? PublishedAdmissionNumber = null,
						  decimal? PercentageFreeSchoolMeals = null,
						  string? PartOfPfiScheme = null,
						  string? ViabilityIssues = null,
						  string? FinancialDeficit = null,
						  string? DiocesanTrust = null,
						  decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = null,
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

						  // legal requirements
						  YesNoNotApplicable? GoverningBodyResolution = null,
						  YesNoNotApplicable? Consultation = null,
						  YesNoNotApplicable? DiocesanConsent = null,
						  YesNoNotApplicable? FoundationConsent = null,
						  bool? LegalRequirementsSectionComplete = null,

						  // school budget info
						  DateTime? EndOfCurrentFinancialYear = null,
						  DateTime? EndOfNextFinancialYear = null,
						  decimal? RevenueCarryForwardAtEndMarchCurrentYear = null,
						  decimal? ProjectedRevenueBalanceAtEndMarchNextYear = null,
						  decimal? CapitalCarryForwardAtEndMarchCurrentYear = null,
						  decimal? CapitalCarryForwardAtEndMarchNextYear = null,
						  string? SchoolBudgetInformationAdditionalInformation = null,
						  bool? SchoolBudgetInformationSectionComplete = null,

						  // pupil schools forecast
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

						  // assigned user
						  User? AssignedUser = null,
						  
						  // Notes
						  ICollection<ProjectNote>? Notes = null)
	{
		this.Urn = Urn;
		this.IfdPipelineId = IfdPipelineId;
		this.SchoolName = SchoolName;
		this.LocalAuthority = LocalAuthority;
		this.ApplicationReferenceNumber = ApplicationReferenceNumber;
		this.ProjectStatus = ProjectStatus;
		this.ApplicationReceivedDate = ApplicationReceivedDate;
		this.AssignedDate = AssignedDate;
		this.HeadTeacherBoardDate = HeadTeacherBoardDate;
		this.OpeningDate = OpeningDate;
		this.BaselineDate = BaselineDate;
		this.LocalAuthorityInformationTemplateSentDate = LocalAuthorityInformationTemplateSentDate;
		this.LocalAuthorityInformationTemplateReturnedDate = LocalAuthorityInformationTemplateReturnedDate;
		this.LocalAuthorityInformationTemplateComments = LocalAuthorityInformationTemplateComments;
		this.LocalAuthorityInformationTemplateLink = LocalAuthorityInformationTemplateLink;
		this.LocalAuthorityInformationTemplateSectionComplete = LocalAuthorityInformationTemplateSectionComplete;
		this.RecommendationForProject = RecommendationForProject;
		this.Author = Author;
		this.Version = Version;
		this.ClearedBy = ClearedBy;
		this.AcademyOrderRequired = AcademyOrderRequired;
		this.PreviousHeadTeacherBoardDateQuestion = PreviousHeadTeacherBoardDateQuestion;
		this.PreviousHeadTeacherBoardDate = PreviousHeadTeacherBoardDate;
		this.PreviousHeadTeacherBoardLink = PreviousHeadTeacherBoardLink;
		this.TrustReferenceNumber = TrustReferenceNumber;
		this.NameOfTrust = NameOfTrust;
		this.SponsorReferenceNumber = SponsorReferenceNumber;
		this.SponsorName = SponsorName;
		this.AcademyTypeAndRoute = AcademyTypeAndRoute;
		this.ProposedAcademyOpeningDate = ProposedAcademyOpeningDate;
		this.SchoolAndTrustInformationSectionComplete = SchoolAndTrustInformationSectionComplete;
		this.ConversionSupportGrantAmount = ConversionSupportGrantAmount;
		this.ConversionSupportGrantChangeReason = ConversionSupportGrantChangeReason;
		this.Region = Region;
		this.SchoolPhase = SchoolPhase;
		this.AgeRange = AgeRange;
		this.SchoolType = SchoolType;
		this.ActualPupilNumbers = ActualPupilNumbers;
		this.Capacity = Capacity;
		this.PublishedAdmissionNumber = PublishedAdmissionNumber;
		this.PercentageFreeSchoolMeals = PercentageFreeSchoolMeals;
		this.PartOfPfiScheme = PartOfPfiScheme;
		this.ViabilityIssues = ViabilityIssues;
		this.FinancialDeficit = FinancialDeficit;
		this.DiocesanTrust = DiocesanTrust;
		this.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust;
		this.DistanceFromSchoolToTrustHeadquarters = DistanceFromSchoolToTrustHeadquarters;
		this.DistanceFromSchoolToTrustHeadquartersAdditionalInformation = DistanceFromSchoolToTrustHeadquartersAdditionalInformation;
		this.MemberOfParliamentParty = MemberOfParliamentParty;
		this.MemberOfParliamentName = MemberOfParliamentName;
		this.GeneralInformationSectionComplete = GeneralInformationSectionComplete;
		this.SchoolPerformanceAdditionalInformation = SchoolPerformanceAdditionalInformation;
		this.RationaleForProject = RationaleForProject;
		this.RationaleForTrust = RationaleForTrust;
		this.RationaleSectionComplete = RationaleSectionComplete;
		this.RisksAndIssues = RisksAndIssues;
		this.EqualitiesImpactAssessmentConsidered = EqualitiesImpactAssessmentConsidered;
		this.RisksAndIssuesSectionComplete = RisksAndIssuesSectionComplete;
		this.GoverningBodyResolution = GoverningBodyResolution;
		this.Consultation = Consultation;
		this.DiocesanConsent = DiocesanConsent;
		this.FoundationConsent = FoundationConsent;
		this.LegalRequirementsSectionComplete = LegalRequirementsSectionComplete;
		this.EndOfCurrentFinancialYear = EndOfCurrentFinancialYear;
		this.EndOfNextFinancialYear = EndOfNextFinancialYear;
		this.RevenueCarryForwardAtEndMarchCurrentYear = RevenueCarryForwardAtEndMarchCurrentYear;
		this.ProjectedRevenueBalanceAtEndMarchNextYear = ProjectedRevenueBalanceAtEndMarchNextYear;
		this.CapitalCarryForwardAtEndMarchCurrentYear = CapitalCarryForwardAtEndMarchCurrentYear;
		this.CapitalCarryForwardAtEndMarchNextYear = CapitalCarryForwardAtEndMarchNextYear;
		this.SchoolBudgetInformationAdditionalInformation = SchoolBudgetInformationAdditionalInformation;
		this.SchoolBudgetInformationSectionComplete = SchoolBudgetInformationSectionComplete;
		this.YearOneProjectedCapacity = YearOneProjectedCapacity;
		this.YearOneProjectedPupilNumbers = YearOneProjectedPupilNumbers;
		this.YearTwoProjectedCapacity = YearTwoProjectedCapacity;
		this.YearTwoProjectedPupilNumbers = YearTwoProjectedPupilNumbers;
		this.YearThreeProjectedCapacity = YearThreeProjectedCapacity;
		this.YearThreeProjectedPupilNumbers = YearThreeProjectedPupilNumbers;
		this.SchoolPupilForecastsAdditionalInformation = SchoolPupilForecastsAdditionalInformation;
		this.KeyStage2PerformanceAdditionalInformation = KeyStage2PerformanceAdditionalInformation;
		this.KeyStage4PerformanceAdditionalInformation = KeyStage4PerformanceAdditionalInformation;
		this.KeyStage5PerformanceAdditionalInformation = KeyStage5PerformanceAdditionalInformation;
		this.AssignedUser = AssignedUser;
		this.Notes = Notes ?? new List<ProjectNote>();
	}

	public int Urn { get; init; }
	public int? IfdPipelineId { get; init; }
	public string? SchoolName { get; init; }
	public string? LocalAuthority { get; init; }
	public string? ApplicationReferenceNumber { get; init; }
	public string? ProjectStatus { get; init; }
	public DateTime? ApplicationReceivedDate { get; init; }
	public DateTime? AssignedDate { get; init; }
	public DateTime? HeadTeacherBoardDate { get; init; }
	public DateTime? OpeningDate { get; init; }
	public DateTime? BaselineDate { get; init; }
	public DateTime? LocalAuthorityInformationTemplateSentDate { get; init; }
	public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; init; }
	public string? LocalAuthorityInformationTemplateComments { get; init; }
	public string? LocalAuthorityInformationTemplateLink { get; init; }
	public bool? LocalAuthorityInformationTemplateSectionComplete { get; init; }
	public string? RecommendationForProject { get; init; }
	public string? Author { get; init; }
	public string? Version { get; init; }
	public string? ClearedBy { get; init; }
	public string? AcademyOrderRequired { get; init; }
	public string? PreviousHeadTeacherBoardDateQuestion { get; init; }
	public DateTime? PreviousHeadTeacherBoardDate { get; init; }
	public string? PreviousHeadTeacherBoardLink { get; init; }
	public string? TrustReferenceNumber { get; init; }
	public string? NameOfTrust { get; init; }
	public string? SponsorReferenceNumber { get; init; }
	public string? SponsorName { get; init; }
	public string? AcademyTypeAndRoute { get; init; }
	public DateTime? ProposedAcademyOpeningDate { get; init; }
	public bool? SchoolAndTrustInformationSectionComplete { get; init; }
	public decimal? ConversionSupportGrantAmount { get; init; }
	public string? ConversionSupportGrantChangeReason { get; init; }
	public string? Region { get; init; }
	public string? SchoolPhase { get; init; }
	public string? AgeRange { get; init; }
	public string? SchoolType { get; init; }
	public int? ActualPupilNumbers { get; init; }
	public int? Capacity { get; init; }
	public string? PublishedAdmissionNumber { get; init; }
	public decimal? PercentageFreeSchoolMeals { get; init; }
	public string? PartOfPfiScheme { get; init; }
	public string? ViabilityIssues { get; init; }
	public string? FinancialDeficit { get; init; }
	public string? DiocesanTrust { get; init; }
	public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; init; }
	public decimal? DistanceFromSchoolToTrustHeadquarters { get; init; }
	public string? DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; init; }
	public string? MemberOfParliamentParty { get; init; }
	public string? MemberOfParliamentName { get; init; }
	public bool? GeneralInformationSectionComplete { get; init; }
	public string? SchoolPerformanceAdditionalInformation { get; init; }
	public string? RationaleForProject { get; init; }
	public string? RationaleForTrust { get; init; }
	public bool? RationaleSectionComplete { get; init; }
	public string? RisksAndIssues { get; init; }
	public string? EqualitiesImpactAssessmentConsidered { get; init; }
	public bool? RisksAndIssuesSectionComplete { get; init; }
	public YesNoNotApplicable? GoverningBodyResolution { get; init; }
	public YesNoNotApplicable? Consultation { get; init; }
	public YesNoNotApplicable? DiocesanConsent { get; init; }
	public YesNoNotApplicable? FoundationConsent { get; init; }
	public bool? LegalRequirementsSectionComplete { get; init; }
	public DateTime? EndOfCurrentFinancialYear { get; init; }
	public DateTime? EndOfNextFinancialYear { get; init; }
	public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; init; }
	public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; init; }
	public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; init; }
	public decimal? CapitalCarryForwardAtEndMarchNextYear { get; init; }
	public string? SchoolBudgetInformationAdditionalInformation { get; init; }
	public bool? SchoolBudgetInformationSectionComplete { get; init; }
	public int? YearOneProjectedCapacity { get; init; }
	public int? YearOneProjectedPupilNumbers { get; init; }
	public int? YearTwoProjectedCapacity { get; init; }
	public int? YearTwoProjectedPupilNumbers { get; init; }
	public int? YearThreeProjectedCapacity { get; init; }
	public int? YearThreeProjectedPupilNumbers { get; init; }
	public string? SchoolPupilForecastsAdditionalInformation { get; init; }
	public string? KeyStage2PerformanceAdditionalInformation { get; init; }
	public string? KeyStage4PerformanceAdditionalInformation { get; init; }
	public string? KeyStage5PerformanceAdditionalInformation { get; init; }
	public User? AssignedUser { get; init; }
	public ICollection<ProjectNote> Notes { get; set; }

	public void Deconstruct(out int Urn,
							out int? IfdPipelineId,
							out string? SchoolName,
							out string? LocalAuthority,
							out string? ApplicationReferenceNumber,
							out string? ProjectStatus,
							out DateTime? ApplicationReceivedDate,
							out DateTime? AssignedDate,
							out DateTime? HeadTeacherBoardDate,
							out DateTime? OpeningDate,
							out DateTime? BaselineDate,

							// la summary page
							out DateTime? LocalAuthorityInformationTemplateSentDate,
							out DateTime? LocalAuthorityInformationTemplateReturnedDate,
							out string? LocalAuthorityInformationTemplateComments,
							out string? LocalAuthorityInformationTemplateLink,
							out bool? LocalAuthorityInformationTemplateSectionComplete,

							// school/trust info
							out string? RecommendationForProject,
							out string? Author,
							out string? Version,
							out string? ClearedBy,
							out string? AcademyOrderRequired,
							out string? PreviousHeadTeacherBoardDateQuestion,
							out DateTime? PreviousHeadTeacherBoardDate,
							out string? PreviousHeadTeacherBoardLink,
							out string? TrustReferenceNumber,
							out string? NameOfTrust,
							out string? SponsorReferenceNumber,
							out string? SponsorName,
							out string? AcademyTypeAndRoute,
							out DateTime? ProposedAcademyOpeningDate,
							out bool? SchoolAndTrustInformationSectionComplete,
							out decimal? ConversionSupportGrantAmount,
							out string? ConversionSupportGrantChangeReason,
							out string? Region,

							// general info
							out string? SchoolPhase,
							out string? AgeRange,
							out string? SchoolType,
							out int? ActualPupilNumbers,
							out int? Capacity,
							out string? PublishedAdmissionNumber,
							out decimal? PercentageFreeSchoolMeals,
							out string? PartOfPfiScheme,
							out string? ViabilityIssues,
							out string? FinancialDeficit,
							out string? DiocesanTrust,
							out decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
							out decimal? DistanceFromSchoolToTrustHeadquarters,
							out string? DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
							out string? MemberOfParliamentParty,
							out string? MemberOfParliamentName,
							out bool? GeneralInformationSectionComplete,

							// school performance ofsted information
							out string? SchoolPerformanceAdditionalInformation,

							// rationale
							out string? RationaleForProject,
							out string? RationaleForTrust,
							out bool? RationaleSectionComplete,

							// risk and issues
							out string? RisksAndIssues,
							out string? EqualitiesImpactAssessmentConsidered,
							out bool? RisksAndIssuesSectionComplete,

							// legal requirements
							out YesNoNotApplicable? GoverningBodyResolution,
							out YesNoNotApplicable? Consultation,
							out YesNoNotApplicable? DiocesanConsent,
							out YesNoNotApplicable? FoundationConsent,
							out bool? LegalRequirementsSectionComplete,

							// school budget info
							out DateTime? EndOfCurrentFinancialYear,
							out DateTime? EndOfNextFinancialYear,
							out decimal? RevenueCarryForwardAtEndMarchCurrentYear,
							out decimal? ProjectedRevenueBalanceAtEndMarchNextYear,
							out decimal? CapitalCarryForwardAtEndMarchCurrentYear,
							out decimal? CapitalCarryForwardAtEndMarchNextYear,
							out string? SchoolBudgetInformationAdditionalInformation,
							out bool? SchoolBudgetInformationSectionComplete,

							// pupil schools forecast
							out int? YearOneProjectedCapacity,
							out int? YearOneProjectedPupilNumbers,
							out int? YearTwoProjectedCapacity,
							out int? YearTwoProjectedPupilNumbers,
							out int? YearThreeProjectedCapacity,
							out int? YearThreeProjectedPupilNumbers,
							out string? SchoolPupilForecastsAdditionalInformation,

							// key stage performance tables
							out string? KeyStage2PerformanceAdditionalInformation,
							out string? KeyStage4PerformanceAdditionalInformation,
							out string? KeyStage5PerformanceAdditionalInformation,

							// assigned user
							out User? AssignedUser,
		
							// Notes
							out ICollection<ProjectNote>? Notes)
	{
		Urn = this.Urn;
		IfdPipelineId = this.IfdPipelineId;
		SchoolName = this.SchoolName;
		LocalAuthority = this.LocalAuthority;
		ApplicationReferenceNumber = this.ApplicationReferenceNumber;
		ProjectStatus = this.ProjectStatus;
		ApplicationReceivedDate = this.ApplicationReceivedDate;
		AssignedDate = this.AssignedDate;
		HeadTeacherBoardDate = this.HeadTeacherBoardDate;
		OpeningDate = this.OpeningDate;
		BaselineDate = this.BaselineDate;
		LocalAuthorityInformationTemplateSentDate = this.LocalAuthorityInformationTemplateSentDate;
		LocalAuthorityInformationTemplateReturnedDate = this.LocalAuthorityInformationTemplateReturnedDate;
		LocalAuthorityInformationTemplateComments = this.LocalAuthorityInformationTemplateComments;
		LocalAuthorityInformationTemplateLink = this.LocalAuthorityInformationTemplateLink;
		LocalAuthorityInformationTemplateSectionComplete = this.LocalAuthorityInformationTemplateSectionComplete;
		RecommendationForProject = this.RecommendationForProject;
		Author = this.Author;
		Version = this.Version;
		ClearedBy = this.ClearedBy;
		AcademyOrderRequired = this.AcademyOrderRequired;
		PreviousHeadTeacherBoardDateQuestion = this.PreviousHeadTeacherBoardDateQuestion;
		PreviousHeadTeacherBoardDate = this.PreviousHeadTeacherBoardDate;
		PreviousHeadTeacherBoardLink = this.PreviousHeadTeacherBoardLink;
		TrustReferenceNumber = this.TrustReferenceNumber;
		NameOfTrust = this.NameOfTrust;
		SponsorReferenceNumber = this.SponsorReferenceNumber;
		SponsorName = this.SponsorName;
		AcademyTypeAndRoute = this.AcademyTypeAndRoute;
		ProposedAcademyOpeningDate = this.ProposedAcademyOpeningDate;
		SchoolAndTrustInformationSectionComplete = this.SchoolAndTrustInformationSectionComplete;
		ConversionSupportGrantAmount = this.ConversionSupportGrantAmount;
		ConversionSupportGrantChangeReason = this.ConversionSupportGrantChangeReason;
		Region = this.Region;
		SchoolPhase = this.SchoolPhase;
		AgeRange = this.AgeRange;
		SchoolType = this.SchoolType;
		ActualPupilNumbers = this.ActualPupilNumbers;
		Capacity = this.Capacity;
		PublishedAdmissionNumber = this.PublishedAdmissionNumber;
		PercentageFreeSchoolMeals = this.PercentageFreeSchoolMeals;
		PartOfPfiScheme = this.PartOfPfiScheme;
		ViabilityIssues = this.ViabilityIssues;
		FinancialDeficit = this.FinancialDeficit;
		DiocesanTrust = this.DiocesanTrust;
		PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = this.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust;
		DistanceFromSchoolToTrustHeadquarters = this.DistanceFromSchoolToTrustHeadquarters;
		DistanceFromSchoolToTrustHeadquartersAdditionalInformation = this.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;
		MemberOfParliamentParty = this.MemberOfParliamentParty;
		MemberOfParliamentName = this.MemberOfParliamentName;
		GeneralInformationSectionComplete = this.GeneralInformationSectionComplete;
		SchoolPerformanceAdditionalInformation = this.SchoolPerformanceAdditionalInformation;
		RationaleForProject = this.RationaleForProject;
		RationaleForTrust = this.RationaleForTrust;
		RationaleSectionComplete = this.RationaleSectionComplete;
		RisksAndIssues = this.RisksAndIssues;
		EqualitiesImpactAssessmentConsidered = this.EqualitiesImpactAssessmentConsidered;
		RisksAndIssuesSectionComplete = this.RisksAndIssuesSectionComplete;
		GoverningBodyResolution = this.GoverningBodyResolution;
		Consultation = this.Consultation;
		DiocesanConsent = this.DiocesanConsent;
		FoundationConsent = this.FoundationConsent;
		LegalRequirementsSectionComplete = this.LegalRequirementsSectionComplete;
		EndOfCurrentFinancialYear = this.EndOfCurrentFinancialYear;
		EndOfNextFinancialYear = this.EndOfNextFinancialYear;
		RevenueCarryForwardAtEndMarchCurrentYear = this.RevenueCarryForwardAtEndMarchCurrentYear;
		ProjectedRevenueBalanceAtEndMarchNextYear = this.ProjectedRevenueBalanceAtEndMarchNextYear;
		CapitalCarryForwardAtEndMarchCurrentYear = this.CapitalCarryForwardAtEndMarchCurrentYear;
		CapitalCarryForwardAtEndMarchNextYear = this.CapitalCarryForwardAtEndMarchNextYear;
		SchoolBudgetInformationAdditionalInformation = this.SchoolBudgetInformationAdditionalInformation;
		SchoolBudgetInformationSectionComplete = this.SchoolBudgetInformationSectionComplete;
		YearOneProjectedCapacity = this.YearOneProjectedCapacity;
		YearOneProjectedPupilNumbers = this.YearOneProjectedPupilNumbers;
		YearTwoProjectedCapacity = this.YearTwoProjectedCapacity;
		YearTwoProjectedPupilNumbers = this.YearTwoProjectedPupilNumbers;
		YearThreeProjectedCapacity = this.YearThreeProjectedCapacity;
		YearThreeProjectedPupilNumbers = this.YearThreeProjectedPupilNumbers;
		SchoolPupilForecastsAdditionalInformation = this.SchoolPupilForecastsAdditionalInformation;
		KeyStage2PerformanceAdditionalInformation = this.KeyStage2PerformanceAdditionalInformation;
		KeyStage4PerformanceAdditionalInformation = this.KeyStage4PerformanceAdditionalInformation;
		KeyStage5PerformanceAdditionalInformation = this.KeyStage5PerformanceAdditionalInformation;
		AssignedUser = this.AssignedUser;
		Notes = this.Notes;
	}

	public bool Equals(ProjectDetails? other)
	{
		if (ReferenceEquals(null, other))
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return Urn == other.Urn && IfdPipelineId == other.IfdPipelineId && string.Equals(SchoolName, other.SchoolName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(LocalAuthority, other.LocalAuthority, StringComparison.InvariantCultureIgnoreCase) && string.Equals(ApplicationReferenceNumber, other.ApplicationReferenceNumber, StringComparison.InvariantCultureIgnoreCase) && string.Equals(ProjectStatus, other.ProjectStatus, StringComparison.InvariantCultureIgnoreCase) && Nullable.Equals(ApplicationReceivedDate, other.ApplicationReceivedDate) && Nullable.Equals(AssignedDate, other.AssignedDate) && Nullable.Equals(HeadTeacherBoardDate, other.HeadTeacherBoardDate) && Nullable.Equals(OpeningDate, other.OpeningDate) && Nullable.Equals(BaselineDate, other.BaselineDate) && Nullable.Equals(LocalAuthorityInformationTemplateSentDate, other.LocalAuthorityInformationTemplateSentDate) && Nullable.Equals(LocalAuthorityInformationTemplateReturnedDate, other.LocalAuthorityInformationTemplateReturnedDate) && string.Equals(LocalAuthorityInformationTemplateComments, other.LocalAuthorityInformationTemplateComments, StringComparison.InvariantCultureIgnoreCase) && string.Equals(LocalAuthorityInformationTemplateLink, other.LocalAuthorityInformationTemplateLink, StringComparison.InvariantCultureIgnoreCase) && LocalAuthorityInformationTemplateSectionComplete == other.LocalAuthorityInformationTemplateSectionComplete && string.Equals(RecommendationForProject, other.RecommendationForProject, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Author, other.Author, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Version, other.Version, StringComparison.InvariantCultureIgnoreCase) && string.Equals(ClearedBy, other.ClearedBy, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AcademyOrderRequired, other.AcademyOrderRequired, StringComparison.InvariantCultureIgnoreCase) && string.Equals(PreviousHeadTeacherBoardDateQuestion, other.PreviousHeadTeacherBoardDateQuestion, StringComparison.InvariantCultureIgnoreCase) && Nullable.Equals(PreviousHeadTeacherBoardDate, other.PreviousHeadTeacherBoardDate) && string.Equals(PreviousHeadTeacherBoardLink, other.PreviousHeadTeacherBoardLink, StringComparison.InvariantCultureIgnoreCase) && string.Equals(TrustReferenceNumber, other.TrustReferenceNumber, StringComparison.InvariantCultureIgnoreCase) && string.Equals(NameOfTrust, other.NameOfTrust, StringComparison.InvariantCultureIgnoreCase) && string.Equals(SponsorReferenceNumber, other.SponsorReferenceNumber, StringComparison.InvariantCultureIgnoreCase) && string.Equals(SponsorName, other.SponsorName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AcademyTypeAndRoute, other.AcademyTypeAndRoute, StringComparison.InvariantCultureIgnoreCase) && Nullable.Equals(ProposedAcademyOpeningDate, other.ProposedAcademyOpeningDate) && SchoolAndTrustInformationSectionComplete == other.SchoolAndTrustInformationSectionComplete && ConversionSupportGrantAmount == other.ConversionSupportGrantAmount && string.Equals(ConversionSupportGrantChangeReason, other.ConversionSupportGrantChangeReason, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Region, other.Region, StringComparison.InvariantCultureIgnoreCase) && string.Equals(SchoolPhase, other.SchoolPhase, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AgeRange, other.AgeRange, StringComparison.InvariantCultureIgnoreCase) && string.Equals(SchoolType, other.SchoolType, StringComparison.InvariantCultureIgnoreCase) && ActualPupilNumbers == other.ActualPupilNumbers && Capacity == other.Capacity && string.Equals(PublishedAdmissionNumber, other.PublishedAdmissionNumber, StringComparison.InvariantCultureIgnoreCase) && PercentageFreeSchoolMeals == other.PercentageFreeSchoolMeals && string.Equals(PartOfPfiScheme, other.PartOfPfiScheme, StringComparison.InvariantCultureIgnoreCase) && string.Equals(ViabilityIssues, other.ViabilityIssues, StringComparison.InvariantCultureIgnoreCase) && string.Equals(FinancialDeficit, other.FinancialDeficit, StringComparison.InvariantCultureIgnoreCase) && string.Equals(DiocesanTrust, other.DiocesanTrust, StringComparison.InvariantCultureIgnoreCase) && PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust == other.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust && DistanceFromSchoolToTrustHeadquarters == other.DistanceFromSchoolToTrustHeadquarters && string.Equals(DistanceFromSchoolToTrustHeadquartersAdditionalInformation, other.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && string.Equals(MemberOfParliamentParty, other.MemberOfParliamentParty, StringComparison.InvariantCultureIgnoreCase) && string.Equals(MemberOfParliamentName, other.MemberOfParliamentName, StringComparison.InvariantCultureIgnoreCase) && GeneralInformationSectionComplete == other.GeneralInformationSectionComplete && string.Equals(SchoolPerformanceAdditionalInformation, other.SchoolPerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && string.Equals(RationaleForProject, other.RationaleForProject, StringComparison.InvariantCultureIgnoreCase) && string.Equals(RationaleForTrust, other.RationaleForTrust, StringComparison.InvariantCultureIgnoreCase) && RationaleSectionComplete == other.RationaleSectionComplete && string.Equals(RisksAndIssues, other.RisksAndIssues, StringComparison.InvariantCultureIgnoreCase) && string.Equals(EqualitiesImpactAssessmentConsidered, other.EqualitiesImpactAssessmentConsidered, StringComparison.InvariantCultureIgnoreCase) && RisksAndIssuesSectionComplete == other.RisksAndIssuesSectionComplete && GoverningBodyResolution == other.GoverningBodyResolution && Consultation == other.Consultation && DiocesanConsent == other.DiocesanConsent && FoundationConsent == other.FoundationConsent && LegalRequirementsSectionComplete == other.LegalRequirementsSectionComplete && Nullable.Equals(EndOfCurrentFinancialYear, other.EndOfCurrentFinancialYear) && Nullable.Equals(EndOfNextFinancialYear, other.EndOfNextFinancialYear) && RevenueCarryForwardAtEndMarchCurrentYear == other.RevenueCarryForwardAtEndMarchCurrentYear && ProjectedRevenueBalanceAtEndMarchNextYear == other.ProjectedRevenueBalanceAtEndMarchNextYear && CapitalCarryForwardAtEndMarchCurrentYear == other.CapitalCarryForwardAtEndMarchCurrentYear && CapitalCarryForwardAtEndMarchNextYear == other.CapitalCarryForwardAtEndMarchNextYear && string.Equals(SchoolBudgetInformationAdditionalInformation, other.SchoolBudgetInformationAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && SchoolBudgetInformationSectionComplete == other.SchoolBudgetInformationSectionComplete && YearOneProjectedCapacity == other.YearOneProjectedCapacity && YearOneProjectedPupilNumbers == other.YearOneProjectedPupilNumbers && YearTwoProjectedCapacity == other.YearTwoProjectedCapacity && YearTwoProjectedPupilNumbers == other.YearTwoProjectedPupilNumbers && YearThreeProjectedCapacity == other.YearThreeProjectedCapacity && YearThreeProjectedPupilNumbers == other.YearThreeProjectedPupilNumbers && string.Equals(SchoolPupilForecastsAdditionalInformation, other.SchoolPupilForecastsAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && string.Equals(KeyStage2PerformanceAdditionalInformation, other.KeyStage2PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && string.Equals(KeyStage4PerformanceAdditionalInformation, other.KeyStage4PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && string.Equals(KeyStage5PerformanceAdditionalInformation, other.KeyStage5PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) && Equals(AssignedUser, other.AssignedUser);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != this.GetType())
		{
			return false;
		}

		return Equals((ProjectDetails)obj);
	}

	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		hashCode.Add(Urn);
		hashCode.Add(IfdPipelineId);
		hashCode.Add(SchoolName, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(LocalAuthority, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ApplicationReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ProjectStatus, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ApplicationReceivedDate);
		hashCode.Add(AssignedDate);
		hashCode.Add(HeadTeacherBoardDate);
		hashCode.Add(OpeningDate);
		hashCode.Add(BaselineDate);
		hashCode.Add(LocalAuthorityInformationTemplateSentDate);
		hashCode.Add(LocalAuthorityInformationTemplateReturnedDate);
		hashCode.Add(LocalAuthorityInformationTemplateComments, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(LocalAuthorityInformationTemplateLink, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(LocalAuthorityInformationTemplateSectionComplete);
		hashCode.Add(RecommendationForProject, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(Author, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(Version, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ClearedBy, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(AcademyOrderRequired, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(PreviousHeadTeacherBoardDateQuestion, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(PreviousHeadTeacherBoardDate);
		hashCode.Add(PreviousHeadTeacherBoardLink, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(TrustReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(NameOfTrust, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(SponsorReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(SponsorName, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(AcademyTypeAndRoute, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ProposedAcademyOpeningDate);
		hashCode.Add(SchoolAndTrustInformationSectionComplete);
		hashCode.Add(ConversionSupportGrantAmount);
		hashCode.Add(ConversionSupportGrantChangeReason, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(Region, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(SchoolPhase, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(AgeRange, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(SchoolType, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ActualPupilNumbers);
		hashCode.Add(Capacity);
		hashCode.Add(PublishedAdmissionNumber, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(PercentageFreeSchoolMeals);
		hashCode.Add(PartOfPfiScheme, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(ViabilityIssues, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(FinancialDeficit, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(DiocesanTrust, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust);
		hashCode.Add(DistanceFromSchoolToTrustHeadquarters);
		hashCode.Add(DistanceFromSchoolToTrustHeadquartersAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(MemberOfParliamentParty, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(MemberOfParliamentName, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(GeneralInformationSectionComplete);
		hashCode.Add(SchoolPerformanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(RationaleForProject, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(RationaleForTrust, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(RationaleSectionComplete);
		hashCode.Add(RisksAndIssues, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(EqualitiesImpactAssessmentConsidered, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(RisksAndIssuesSectionComplete);
		hashCode.Add(GoverningBodyResolution);
		hashCode.Add(Consultation);
		hashCode.Add(DiocesanConsent);
		hashCode.Add(FoundationConsent);
		hashCode.Add(LegalRequirementsSectionComplete);
		hashCode.Add(EndOfCurrentFinancialYear);
		hashCode.Add(EndOfNextFinancialYear);
		hashCode.Add(RevenueCarryForwardAtEndMarchCurrentYear);
		hashCode.Add(ProjectedRevenueBalanceAtEndMarchNextYear);
		hashCode.Add(CapitalCarryForwardAtEndMarchCurrentYear);
		hashCode.Add(CapitalCarryForwardAtEndMarchNextYear);
		hashCode.Add(SchoolBudgetInformationAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(SchoolBudgetInformationSectionComplete);
		hashCode.Add(YearOneProjectedCapacity);
		hashCode.Add(YearOneProjectedPupilNumbers);
		hashCode.Add(YearTwoProjectedCapacity);
		hashCode.Add(YearTwoProjectedPupilNumbers);
		hashCode.Add(YearThreeProjectedCapacity);
		hashCode.Add(YearThreeProjectedPupilNumbers);
		hashCode.Add(SchoolPupilForecastsAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(KeyStage2PerformanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(KeyStage4PerformanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(KeyStage5PerformanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
		hashCode.Add(AssignedUser);
		return hashCode.ToHashCode();
	}
}
