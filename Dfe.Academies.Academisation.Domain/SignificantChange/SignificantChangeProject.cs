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

		public void SetAdmissionVariationConsultation(bool? consultationIncludeAdmissionVariation,
			string? noAdmissionVariationReason)
		{
			Details.ConsultationIncludeAdmissionVariation = consultationIncludeAdmissionVariation;
			Details.ConsultationNoAdmissionVariationReason = consultationIncludeAdmissionVariation is false
				? noAdmissionVariationReason
				: null;

			SetProjectTier();
		}

		public void SetStakeholderConsultation(bool? trustConsultedStakeholders,
			string? trustConsultedStakeholdersNotConsultedReason)
		{
			Details.TrustConsultedStakeholders = trustConsultedStakeholders;
			Details.TrustConsultedStakeholdersNotConsultedReason = trustConsultedStakeholders is false
				? trustConsultedStakeholdersNotConsultedReason
				: null;

			SetProjectTier();
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

			SetProjectTier();
		}

		public void SetFunding(FundingAnswer? fundingAnswer, string? additionalInformation, string? supportingEvidence)
		{
			Details.FundingAnswer = fundingAnswer;
			Details.FundingAdditionalInformation = fundingAnswer is FundingAnswer.No
				? additionalInformation
				: null;
			Details.FundingSupportingEvidence = supportingEvidence;

			SetProjectTier();
		}

		public void SetReligiousBodyConsultation(bool? trustConsultedReligiousBody,
					string? trustConsultedReligiousBodyNotConsultedReason)
		{
			Details.TrustConsultedReligiousBody = trustConsultedReligiousBody;
			Details.TrustConsultedReligiousBodyNotConsultedReason = trustConsultedReligiousBody is false
				? trustConsultedReligiousBodyNotConsultedReason
				: null;

			SetProjectTier();
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

		public void SetEqualitiesImpactAssessment(bool? equalitiesImpactAssessmentCompleted,
			EqualitiesImpact? equalitiesImpactIdentified, string? equalitiesImpactIdentifiedMitigation)
		{
			Details.EqualitiesImpactAssessmentCompleted = equalitiesImpactAssessmentCompleted;
			Details.EqualitiesImpactIdentified = equalitiesImpactIdentified;
			Details.EqualitiesImpactIdentifiedMitigation = equalitiesImpactIdentifiedMitigation;

			SetProjectTier();

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

			SetProjectTier();
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

			SetProjectTier();
		}

		public void SetProjectTier()
		{
			bool shouldMoveToTierTwo =
				// Has a 3 week consultation been completed? = NO
				Details.ConsultationLastedMinimumThreeWeeks == ConsultationDurationAnswer.No
				// Has the trust consulted stakeholders about the planned significant change? = NO
				|| Details.TrustConsultedStakeholders == false
				// Were there any objections raised?= NO - dont think thats right so set it to yesNoFurtherInformationProvided
				|| Details.StakeholderObjections == SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided
				// Have the relevant stakeholders been notified of the admissions changes? = NO
				|| Details.ConsultationIncludeAdmissionVariation == false
				// Have any relevant religious bodies been consulted about this significant change? = NO
				|| Details.TrustConsultedReligiousBody == false
				// Has the Public Sector Equality Duty been considered and an Equalities Impact Assessment been completed? = NO
				|| Details.EqualitiesImpactAssessmentCompleted == false
				// Is the decision likely to disproportionately affect any particular person or group who share protected characteristics? = LIKELY - this doesn't currently have likely as a value
				// Details.EqualitiesImpactIdentified == EqualitiesImpact.ImpactsIdentified ||
				// Has planning permission been secured? = NO
				|| Details.PlanningPermission == PlanningPermissionAnswer.No
				// Has funding been secured? = NO
				|| Details.FundingAnswer == FundingAnswer.No;

			if (shouldMoveToTierTwo)
			{
				MoveToTierTwoIfApplicable();
				return;
			}

			Tier = 1;
		}
	}
}
