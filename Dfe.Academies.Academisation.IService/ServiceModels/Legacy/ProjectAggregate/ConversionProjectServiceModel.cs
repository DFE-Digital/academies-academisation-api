﻿using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Newtonsoft.Json;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public sealed class ConversionProjectServiceModel : IEquatable<ConversionProjectServiceModel>
	{
		public ConversionProjectServiceModel(int id, int urn)
		{
			Id = id;
			Urn = urn;
		}

		public int Id { get; init; }
		public int? Urn { get; init; }
		
		public int? TrustUkprn { get; init; }

		public int? FormAMatProjectId { get; init; }
		public Guid? SchoolSharePointId { get; init; }
		public bool? IsFormAMat { get; init; }
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
		public Guid? ApplicationSharePointId { get; set; }
		public DateTime? ProposedConversionDate { get; set; }
		public bool? ProjectDatesSectionComplete { get; set; }
		public bool IsReadOnly { get; set; }
		
		public DateTime? ProjectSentToCompleteDate { get; set; }
		
		

		public bool Equals(ConversionProjectServiceModel? other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Id == other.Id && Urn == other.Urn && IfdPipelineId == other.IfdPipelineId &&
				   string.Equals(SchoolName, other.SchoolName, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(LocalAuthority, other.LocalAuthority, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ApplicationReferenceNumber, other.ApplicationReferenceNumber,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ProjectStatus, other.ProjectStatus, StringComparison.InvariantCultureIgnoreCase) &&
				   Nullable.Equals(ApplicationReceivedDate, other.ApplicationReceivedDate) &&
				   Nullable.Equals(AssignedDate, other.AssignedDate) &&
				   Nullable.Equals(HeadTeacherBoardDate, other.HeadTeacherBoardDate) &&
				   Nullable.Equals(BaselineDate, other.BaselineDate) &&
				   Nullable.Equals(LocalAuthorityInformationTemplateSentDate,
					   other.LocalAuthorityInformationTemplateSentDate) &&
				   Nullable.Equals(LocalAuthorityInformationTemplateReturnedDate,
					   other.LocalAuthorityInformationTemplateReturnedDate) &&
				   string.Equals(LocalAuthorityInformationTemplateComments,
					   other.LocalAuthorityInformationTemplateComments, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(LocalAuthorityInformationTemplateLink, other.LocalAuthorityInformationTemplateLink,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   LocalAuthorityInformationTemplateSectionComplete ==
				   other.LocalAuthorityInformationTemplateSectionComplete &&
				   string.Equals(RecommendationForProject, other.RecommendationForProject,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(Author, other.Author, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(Version, other.Version, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ClearedBy, other.ClearedBy, StringComparison.InvariantCultureIgnoreCase) &&
				   Nullable.Equals(DaoPackSentDate, other.DaoPackSentDate) &&
			   string.Equals(Form7Received, other.Form7Received, StringComparison.InvariantCultureIgnoreCase) &&
			   Nullable.Equals(Form7ReceivedDate, other.Form7ReceivedDate) &&
				   string.Equals(PreviousHeadTeacherBoardDateQuestion, other.PreviousHeadTeacherBoardDateQuestion,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   Nullable.Equals(PreviousHeadTeacherBoardDate, other.PreviousHeadTeacherBoardDate) &&
				   string.Equals(PreviousHeadTeacherBoardLink, other.PreviousHeadTeacherBoardLink,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(TrustReferenceNumber, other.TrustReferenceNumber,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(NameOfTrust, other.NameOfTrust, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(SponsorReferenceNumber, other.SponsorReferenceNumber,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(SponsorName, other.SponsorName, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(AcademyTypeAndRoute, other.AcademyTypeAndRoute,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   Nullable.Equals(ProposedConversionDate, other.ProposedConversionDate) &&
				   SchoolAndTrustInformationSectionComplete == other.SchoolAndTrustInformationSectionComplete &&
				   ConversionSupportGrantAmount == other.ConversionSupportGrantAmount &&
				   string.Equals(ConversionSupportGrantChangeReason, other.ConversionSupportGrantChangeReason,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ConversionSupportGrantType, other.ConversionSupportGrantType,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ConversionSupportGrantEnvironmentalImprovementGrant, other.ConversionSupportGrantEnvironmentalImprovementGrant,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(Region, other.Region, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(SchoolPhase, other.SchoolPhase, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(AgeRange, other.AgeRange, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(SchoolType, other.SchoolType, StringComparison.InvariantCultureIgnoreCase) &&
				   ActualPupilNumbers == other.ActualPupilNumbers && Capacity == other.Capacity &&
				   string.Equals(PublishedAdmissionNumber, other.PublishedAdmissionNumber,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   PercentageFreeSchoolMeals == other.PercentageFreeSchoolMeals &&
				   string.Equals(PartOfPfiScheme, other.PartOfPfiScheme, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(ViabilityIssues, other.ViabilityIssues, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(FinancialDeficit, other.FinancialDeficit,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(DiocesanTrust, other.DiocesanTrust, StringComparison.InvariantCultureIgnoreCase) &&
				   PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust ==
				   other.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust &&
				   DistanceFromSchoolToTrustHeadquarters == other.DistanceFromSchoolToTrustHeadquarters &&
				   string.Equals(DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
					   other.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(MemberOfParliamentNameAndParty, other.MemberOfParliamentNameAndParty,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   SchoolOverviewSectionComplete == other.SchoolOverviewSectionComplete &&
				   string.Equals(SchoolPerformanceAdditionalInformation, other.SchoolPerformanceAdditionalInformation,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(RationaleForProject, other.RationaleForProject,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(RationaleForTrust, other.RationaleForTrust,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   RationaleSectionComplete == other.RationaleSectionComplete &&
				   string.Equals(RisksAndIssues, other.RisksAndIssues, StringComparison.InvariantCultureIgnoreCase) &&
				   RisksAndIssuesSectionComplete == other.RisksAndIssuesSectionComplete &&
				   GoverningBodyResolution == other.GoverningBodyResolution && Consultation == other.Consultation &&
				   DiocesanConsent == other.DiocesanConsent && FoundationConsent == other.FoundationConsent &&
				   LegalRequirementsSectionComplete == other.LegalRequirementsSectionComplete &&
				   Nullable.Equals(EndOfCurrentFinancialYear, other.EndOfCurrentFinancialYear) &&
				   Nullable.Equals(EndOfNextFinancialYear, other.EndOfNextFinancialYear) &&
				   RevenueCarryForwardAtEndMarchCurrentYear == other.RevenueCarryForwardAtEndMarchCurrentYear &&
				   ProjectedRevenueBalanceAtEndMarchNextYear == other.ProjectedRevenueBalanceAtEndMarchNextYear &&
				   CapitalCarryForwardAtEndMarchCurrentYear == other.CapitalCarryForwardAtEndMarchCurrentYear &&
				   CapitalCarryForwardAtEndMarchNextYear == other.CapitalCarryForwardAtEndMarchNextYear &&
				   string.Equals(SchoolBudgetInformationAdditionalInformation,
					   other.SchoolBudgetInformationAdditionalInformation,
					   StringComparison.InvariantCultureIgnoreCase) &&
				   SchoolBudgetInformationSectionComplete == other.SchoolBudgetInformationSectionComplete &&
				   YearOneProjectedCapacity == other.YearOneProjectedCapacity &&
				   YearOneProjectedPupilNumbers == other.YearOneProjectedPupilNumbers &&
				   YearTwoProjectedCapacity == other.YearTwoProjectedCapacity &&
				   YearTwoProjectedPupilNumbers == other.YearTwoProjectedPupilNumbers &&
				   YearThreeProjectedCapacity == other.YearThreeProjectedCapacity &&
				   YearThreeProjectedPupilNumbers == other.YearThreeProjectedPupilNumbers &&
				   string.Equals(SchoolPupilForecastsAdditionalInformation,
					   other.SchoolPupilForecastsAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(KeyStage2PerformanceAdditionalInformation,
					   other.KeyStage2PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(KeyStage4PerformanceAdditionalInformation,
					   other.KeyStage4PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) &&
				   string.Equals(KeyStage5PerformanceAdditionalInformation,
					   other.KeyStage5PerformanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) &&
					string.Equals(EducationalAttendanceAdditionalInformation,
					   other.EducationalAttendanceAdditionalInformation, StringComparison.InvariantCultureIgnoreCase) &&
				   Equals(AssignedUser, other.AssignedUser) &&
									  string.Equals(ExternalApplicationFormUrl,
					   other.ExternalApplicationFormUrl, StringComparison.InvariantCultureIgnoreCase) &&
					   ExternalApplicationFormSaved == other.ExternalApplicationFormSaved &&
					   IsReadOnly == other.IsReadOnly &&
				   ProjectSentToCompleteDate == other.ProjectSentToCompleteDate;
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

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((ConversionProjectServiceModel)obj);
		}

		public override int GetHashCode()
		{
			var hashCode = new HashCode();
			hashCode.Add(Id);
			hashCode.Add(Urn);
			hashCode.Add(IfdPipelineId);
			hashCode.Add(SchoolName, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(LocalAuthority, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ApplicationReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ProjectStatus, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ApplicationReceivedDate);
			hashCode.Add(AssignedDate);
			hashCode.Add(HeadTeacherBoardDate);
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
			hashCode.Add(DaoPackSentDate);
			hashCode.Add(Form7Received, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(Form7ReceivedDate);
			hashCode.Add(PreviousHeadTeacherBoardDateQuestion, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(PreviousHeadTeacherBoardDate);
			hashCode.Add(PreviousHeadTeacherBoardLink, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(TrustReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(NameOfTrust, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(SponsorReferenceNumber, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(SponsorName, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(AcademyTypeAndRoute, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ProposedConversionDate);
			hashCode.Add(SchoolAndTrustInformationSectionComplete);
			hashCode.Add(ConversionSupportGrantAmount);
			hashCode.Add(ConversionSupportGrantChangeReason, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ConversionSupportGrantType, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ConversionSupportGrantEnvironmentalImprovementGrant, StringComparer.InvariantCultureIgnoreCase);
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
			hashCode.Add(DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(MemberOfParliamentNameAndParty, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(SchoolOverviewSectionComplete);
			hashCode.Add(SchoolPerformanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(RationaleForProject, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(RationaleForTrust, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(RationaleSectionComplete);
			hashCode.Add(RisksAndIssues, StringComparer.InvariantCultureIgnoreCase);
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
			hashCode.Add(EducationalAttendanceAdditionalInformation, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(AssignedUser);
			hashCode.Add(CreatedOn);
			hashCode.Add(ExternalApplicationFormUrl, StringComparer.InvariantCultureIgnoreCase);
			hashCode.Add(ExternalApplicationFormSaved);
			hashCode.Add(IsReadOnly);
			hashCode.Add(ProjectSentToCompleteDate);
			return hashCode.ToHashCode();
		}
	}
}
