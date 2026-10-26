using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{

	public record SignificantChangeProjectOptions(
		int urn,
		byte tier,
		string trustName,
		string trustUkprn,
		string typeOfSignificantChange,
		string schoolName,
		string? localAuthorityName = null,
		string? companiesHouseNumber = null,
		string? regionName = null
	);


	public class SignificantChangeProject : Entity, IAggregateRoot
	{
		// Private constructor for EF Core
		private SignificantChangeProject()
		{
		}

		public SignificantChangeProject(SignificantChangeStatus status, SignificantChangeProjectOptions options)
		{
			Status = status;
			Urn = options.urn;
			SchoolName = options.schoolName;
			Tier = options.tier;
			TrustName = options.trustName;
			TrustUkprn = options.trustUkprn;
			TypeOfSignificantChange = options.typeOfSignificantChange;
			LocalAuthorityName = options.localAuthorityName;
			CompaniesHouseNumber = options.companiesHouseNumber;
			RegionName = options.regionName;
		}

		public SignificantChangeStatus Status { get; private set; }
		public int Urn { get; private set; }
		public string SchoolName { get; private set; } = string.Empty;
		public byte Tier { get; private set; }
		public Guid? AssignedUserId { get; private set; }
		public string? AssignedUserFullName { get; private set; }
		public string? AssignedUserEmailAddress { get; private set; }
		public string TrustName { get; private set; } = string.Empty;
		public string TrustUkprn { get; private set; } = string.Empty;
		public string TypeOfSignificantChange { get; private set; } = string.Empty;
		public string? ApplicationId { get; set; }
		public string? ApplicationReference { get; set; }
		public DateTime? ReadOnlyDate { get; private set; }
		public SignificantChangeProjectDetails Details { get; private set; } = new();
		public string? LocalAuthorityName { get; private set; }
		public string? CompaniesHouseNumber { get; private set; }
		public bool ProjectSentToComplete { get; private set; } = false;
		public Guid? CompleteProjectId { get; private set; }
		public string? RegionName { get; private set; }

		public void AssignUser(Guid userId, string userEmail, string userFullName)
		{
			AssignedUserId = userId;
			AssignedUserEmailAddress = userEmail;
			AssignedUserFullName = userFullName;
		}

		public void SetAdmissionVariationDetails(
			bool? admissionsVariationRequired,
			string? supportingEvidence,
			bool? admissionsVariationLocalAuthoritySupportive,
			bool? admissionsVariationPupilPlacePlanningSupportive,
			bool? admissionsVariationAdmissionsTeamSupportive,
			string? admissionsVariationLocalAuthoritySupportiveAdditionalInformation,
			string? admissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
			string? admissionsVariationAdmissionsTeamSupportiveAdditionalInformation)
		{
			Details.AdmissionsVariationRequired = admissionsVariationRequired;
			Details.AdmissionsVariationSupportingEvidence = supportingEvidence;
			Details.AdmissionsVariationLocalAuthoritySupportive = admissionsVariationLocalAuthoritySupportive;
			Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation =
				admissionsVariationLocalAuthoritySupportive is false
					? admissionsVariationLocalAuthoritySupportiveAdditionalInformation
					: null;
			Details.AdmissionsVariationPupilPlacePlanningSupportive = admissionsVariationPupilPlacePlanningSupportive;
			Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation =
				admissionsVariationPupilPlacePlanningSupportive is false
					? admissionsVariationPupilPlacePlanningSupportiveAdditionalInformation
					: null;
			Details.AdmissionsVariationAdmissionsTeamSupportive = admissionsVariationAdmissionsTeamSupportive;
			Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation =
				admissionsVariationAdmissionsTeamSupportive is false
					? admissionsVariationAdmissionsTeamSupportiveAdditionalInformation
					: null;

			if ( admissionsVariationLocalAuthoritySupportive is false 
				|| admissionsVariationPupilPlacePlanningSupportive is false 
				|| admissionsVariationAdmissionsTeamSupportive is false)
			{
				MoveToTierTwoIfApplicable();
			}
		}

		public void SetAdmissionVariationConsultation(bool? consultationIncludeAdmissionVariation,
			string? noAdmissionVariationReason)
		{
			Details.ConsultationIncludeAdmissionVariation = consultationIncludeAdmissionVariation;
			Details.ConsultationNoAdmissionVariationReason = consultationIncludeAdmissionVariation is false
				? noAdmissionVariationReason
				: null;

			if (consultationIncludeAdmissionVariation is false)
			{
				MoveToTierTwoIfApplicable();
			}
		}

		public void SetStakeholderConsultation(bool? trustConsultedStakeholders,
			string? trustConsultedStakeholdersNotConsultedReason)
		{
			Details.TrustConsultedStakeholders = trustConsultedStakeholders;
			Details.TrustConsultedStakeholdersNotConsultedReason = trustConsultedStakeholders is false
				? trustConsultedStakeholdersNotConsultedReason
				: null;

			if (trustConsultedStakeholders is false)
				MoveToTierTwoIfApplicable();
		}

		public void SetConsultationDuration(
			ConsultationDurationAnswer? consultationLastedMinimumThreeWeeks,
			string? consultationDurationNotMetReason)
		{
			Details.ConsultationLastedMinimumThreeWeeks = consultationLastedMinimumThreeWeeks;
			Details.ConsultationDurationNotMetReason =
				consultationLastedMinimumThreeWeeks is ConsultationDurationAnswer.No
					? consultationDurationNotMetReason
					: null;

			if (consultationLastedMinimumThreeWeeks is ConsultationDurationAnswer.No)
				MoveToTierTwoIfApplicable();
		}

		public void SetFunding(FundingAnswer? fundingAnswer, string? additionalInformation, string? supportingEvidence)
		{
			Details.FundingAnswer = fundingAnswer;
			Details.FundingAdditionalInformation = fundingAnswer is FundingAnswer.No
				? additionalInformation
				: null;
			Details.FundingSupportingEvidence = supportingEvidence;
		}

		public void SetHqtfConsideration(
			HqtfConsiderationAnswer? hqtfConsiderationAnswer,
			string? additionalInformation,
			string? supportingEvidence)
		{
			Details.HqtfConsiderationAnswer = hqtfConsiderationAnswer;
			Details.HqtfConsiderationAdditionalInformation = hqtfConsiderationAnswer is HqtfConsiderationAnswer.Yes
				? additionalInformation
				: null;
			Details.HqtfConsiderationSupportingEvidence = supportingEvidence;
		}

		public void SetReligiousBodyConsultation(bool? trustConsultedReligiousBody,
					string? trustConsultedReligiousBodyNotConsultedReason)
		{
			Details.TrustConsultedReligiousBody = trustConsultedReligiousBody;
			Details.TrustConsultedReligiousBodyNotConsultedReason = trustConsultedReligiousBody is false
				? trustConsultedReligiousBodyNotConsultedReason
				: null;

			if (trustConsultedReligiousBody is false)
				MoveToTierTwoIfApplicable();
		}

		public void SetReligiousBodyConsultation(
			YesNoNotApplicableAnswer? trustConsultedReligiousBody,
			string? trustConsultedReligiousBodyNotConsultedReason,
			YesNoNotApplicableAnswer? religiousBodyRaisedObjection,
			string? religiousBodyRaisedObjectionReason,
			YesNoNotApplicableAnswer? religiousBodyConsentGranted,
			string? religiousBodyConsentNotGrantedReason,
			string? supportingEvidence)
		{
			Details.TrustConsultedReligiousBodyAnswer = trustConsultedReligiousBody;
			Details.TrustConsultedReligiousBodyNotConsultedReason =
				trustConsultedReligiousBody is YesNoNotApplicableAnswer.No ? trustConsultedReligiousBodyNotConsultedReason : null;
			Details.ReligiousBodyRaisedObjection = religiousBodyRaisedObjection;
			Details.ReligiousBodyRaisedObjectionReason =
				religiousBodyRaisedObjection is YesNoNotApplicableAnswer.No ? religiousBodyRaisedObjectionReason : null;
			Details.ReligiousBodyConsentGranted = religiousBodyConsentGranted;
			Details.ReligiousBodyConsentNotGrantedReason =
				religiousBodyConsentGranted is YesNoNotApplicableAnswer.No ? religiousBodyConsentNotGrantedReason : null;
			Details.ReligiousBodySupportingEvidence = supportingEvidence;

			if (trustConsultedReligiousBody is YesNoNotApplicableAnswer.No)
				MoveToTierTwoIfApplicable();
		}

		public void SetOtherStakeholderEngagement(
			bool? trustConsultedStakeholders,
			string? trustConsultedStakeholdersNotConsultedReason,
			bool? wereThereAnyObjectionsRaised,
			string? stakeholderObjectionsDetails,
			YesNoNotApplicableAnswer? relevantStakeholdersNotifiedOfAdmissionsChanges,
			string? relevantStakeholdersNotifiedOfAdmissionsChangesReason,
			string? supportingEvidence)
		{
			Details.TrustConsultedStakeholders = trustConsultedStakeholders;
			Details.TrustConsultedStakeholdersNotConsultedReason =
				trustConsultedStakeholders is false ? trustConsultedStakeholdersNotConsultedReason : null;
			Details.WereThereAnyObjectionsRaised = wereThereAnyObjectionsRaised;
			Details.StakeholderObjectionsDetails =
				wereThereAnyObjectionsRaised is true ? stakeholderObjectionsDetails : null;
			Details.RelevantStakeholdersNotifiedOfAdmissionsChanges = relevantStakeholdersNotifiedOfAdmissionsChanges;
			Details.RelevantStakeholdersNotifiedOfAdmissionsChangesReason =
				relevantStakeholdersNotifiedOfAdmissionsChanges is YesNoNotApplicableAnswer.No
					? relevantStakeholdersNotifiedOfAdmissionsChangesReason
					: null;
			Details.OtherStakeholderEngagementSupportingEvidence = supportingEvidence;

			if (trustConsultedStakeholders is false)
				MoveToTierTwoIfApplicable();
		}

		public void MoveToTierTwoIfApplicable()
		{
			if (Tier == 1) Tier = 2;
		}

		public static SignificantChangeProject Create(SignificantChangeProjectOptions options, DateTime createdOn)
		{
			return new SignificantChangeProject(SignificantChangeStatus.PreDecision, options) { CreatedOn = createdOn };
		}

		public void SetReadOnlyDate(DateTime readOnlyDate)
		{
			this.ReadOnlyDate = readOnlyDate;
		}

		public void SetEqualitiesImpactAssessment(bool? equalitiesImpactAssessmentCompleted, string? equalitiesImpactAssessmentNoReason,
			EqualitiesImpact? equalitiesImpactIdentified, string? equalitiesLikelyDetails, string? equalitiesSomeImpactDetails,
			string? equalitiesImpactSupportingEvidence)
		{
			Details.EqualitiesImpactAssessmentCompleted = equalitiesImpactAssessmentCompleted;
			Details.EqualitiesImpactAssessmentNoReason = equalitiesImpactAssessmentCompleted is false ? equalitiesImpactAssessmentNoReason : null;
			Details.EqualitiesImpactIdentified = equalitiesImpactIdentified;
			Details.EqualitiesLikelyDetails = equalitiesLikelyDetails;
			Details.EqualitiesSomeImpactDetails = equalitiesSomeImpactDetails;
			Details.EqualitiesImpactSupportingEvidence = equalitiesImpactSupportingEvidence;

		}

		public void SetProjectDates(DateTime? proposedDecisionDate, DateTime? proposedChangeDate)
		{
			Details.ProposedDecisionDate = proposedDecisionDate;
			Details.ProposedChangeDate = proposedChangeDate;
		}

		public void SetStakeholderObjections(SignificantChangeStakeholderObjections? stakeholderObjections, string? stakeholderObjectionsComment)
		{
			Details.StakeholderObjections = stakeholderObjections;
			Details.StakeholderObjectionsComment = stakeholderObjections == SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided ? stakeholderObjectionsComment : null;

			
			if (stakeholderObjections is SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided)
				MoveToTierTwoIfApplicable();
		}

		public void SetProjectSentToComplete(Guid? completeProjectId)
		{
			ProjectSentToComplete = true;
			CompleteProjectId = completeProjectId;
    }
      
		public void SetStatus(SignificantChangeStatus requestStatus)
		{
			Status = requestStatus;
		}

		public void SetPlanningPermission(PlanningPermissionAnswer planningPermissionAnswer,
			string? additionalInformation, string? supportingEvidence)
		{
			Details.PlanningPermission = planningPermissionAnswer;
			Details.PlanningPermissionAdditionalInformation = additionalInformation;
			Details.PlanningPermissionSupportingEvidence = supportingEvidence;
		}
	}
}
