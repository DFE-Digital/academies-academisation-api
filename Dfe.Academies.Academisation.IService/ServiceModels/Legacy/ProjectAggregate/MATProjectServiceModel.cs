using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Newtonsoft.Json;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public sealed class MATProjectServiceModel
	{
		public MATProjectServiceModel(int id, int urn)
		{
			Id = id;
			Urn = urn;
		}

		public int Id { get; init; }
		public int? Urn { get; init; }
		[JsonIgnore] public int? IfdPipelineId { get; init; }
		public string? SchoolName { get; init; }
		public string? LocalAuthority { get; init; }
		public string? ApplicationReferenceNumber { get; init; }
		public string? ProjectStatus { get; init; }
		public DateTime? ApplicationReceivedDate { get; init; }
		public DateTime? AssignedDate { get; init; }
		public DateTime? HeadTeacherBoardDate { get; init; }
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
		public DateTime? DaoPackSentDate { get; init; }
		public string? Form7Received { get; init; }
		public DateTime? Form7ReceivedDate { get; init; }
		public bool? AnnexBFormReceived { get; init; }
		public string? AnnexBFormUrl { get; init; }
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
		public string? ConversionSupportGrantType { get; init; }
		public string? ConversionSupportGrantEnvironmentalImprovementGrant { get; init; }
		public bool? ConversionSupportGrantAmountChanged { get; init; }
		public string? ConversionSupportGrantNumberOfSites { get; set; }
		public string? Region { get; init; }
		public string? SchoolPhase { get; init; }
		[JsonIgnore] public string? AgeRange { get; init; }
		public string? SchoolType { get; init; }
		[JsonIgnore] public int? ActualPupilNumbers { get; init; }
		[JsonIgnore] public int? Capacity { get; init; }
		public string? PublishedAdmissionNumber { get; init; }
		[JsonIgnore] public decimal? PercentageFreeSchoolMeals { get; init; }
		public string? PartOfPfiScheme { get; init; }
		public string? PfiSchemeDetails { get; init; }
		public string? ViabilityIssues { get; init; }
		public decimal? NumberOfPlacesFundedFor { get; init; }
		public decimal? NumberOfResidentialPlaces { get; init; }
		public decimal? NumberOfFundedResidentialPlaces { get; init; }
		public string? FinancialDeficit { get; init; }
		[JsonIgnore] public string? DiocesanTrust { get; init; }
		[JsonIgnore] public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; init; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; init; }
		public string? DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; init; }
		public string? MemberOfParliamentNameAndParty { get; init; }
		public bool? SchoolOverviewSectionComplete { get; init; }
		public string? SchoolPerformanceAdditionalInformation { get; init; }
		public string? RationaleForProject { get; init; }
		public string? RationaleForTrust { get; init; }
		public bool? RationaleSectionComplete { get; init; }
		public string? RisksAndIssues { get; init; }
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
		public string? EducationalAttendanceAdditionalInformation { get; init; }

		public User? AssignedUser { get; init; }
		public ICollection<ConversionProjectDeleteNote>? Notes { get; set; }
		public DateTime CreatedOn { get; set; }
		public bool? ExternalApplicationFormSaved { get; init; }
		public string? ExternalApplicationFormUrl { get; init; }
		public bool? PupilsAttendingGroupPermanentlyExcluded { get; init; }
		public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; init; }
		public bool? PupilsAttendingGroupTeenageMums { get; init; }
		public int? NumberOfAlternativeProvisionPlaces { get; set; }
		public int? NumberOfMedicalPlaces { get; set; }
		public int? NumberOfSENUnitPlaces { get; set; }
		public int? NumberOfPost16Places { get; set; }
	}
}
