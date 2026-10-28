using Dfe.Academies.Academisation.Domain.Core.SignificantChange;

namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	public YesNoNotApplicableAnswer? TrustConsultedReligiousBodyAnswer { get; set; }
	public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; }
	public string? ReligiousBodyRaisedObjectionReason { get; set; }
	public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; }
	public string? ReligiousBodyConsentNotGrantedReason { get; set; }
	public string? ReligiousBodySupportingEvidence { get; set; }
	public bool? WereThereAnyObjectionsRaised { get; set; }
	public string? StakeholderObjectionsDetails { get; set; }
	public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; }
	public string? RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; }
	public string? OtherStakeholderEngagementSupportingEvidence { get; set; }
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }
	public bool? EqualitiesImpactAssessmentCompleted { get; set; }
	public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
	public string? EqualitiesLikelyDetails { get; set; }
	public string? EqualitiesSomeImpactDetails { get; set; }
	public string? EqualitiesImpactSupportingEvidence { get; set; }
	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }
	public FundingAnswer? FundingAnswer { get; set; }
	public string? FundingAdditionalInformation { get; set; }
	public string? FundingSupportingEvidence { get; set; }
	public HqtfConsiderationAnswer? HqtfConsiderationAnswer { get; set; }
	public string? HqtfConsiderationAdditionalInformation { get; set; }
	public string? HqtfConsiderationSupportingEvidence { get; set; }
	public bool? FhaRisksOrIssuesRaised { get; set; }
	public string? FhaRisksOrIssuesRaisedAdditionalInformation { get; set; }
	public string? FhaRisksOrIssuesRaisedSupportingEvidence { get; set; }
	public PlanningPermissionAnswer? PlanningPermission { get; set; }
	public string? PlanningPermissionSupportingEvidence { get; set; }
	public string? PlanningPermissionAdditionalInformation { get; set; }
	public string? EqualitiesImpactAssessmentNoReason { get; set; }
	public DateTime? OfstedInspectionDate { get; set; }
	public bool? MeetsOfstedStandards { get; set; }
	public string? OfstedAdditionalInformation { get; set; }
	public string? OfstedSupportingEvidence { get; set; }

	public bool? AdmissionsVariationRequired { get; set; }
	public string? AdmissionsVariationSupportingEvidence { get; set; }
	public bool? AdmissionsVariationLocalAuthoritySupportive { get; set; }
	public string? AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation { get; set; }
	public bool? AdmissionsVariationPupilPlacePlanningSupportive { get; set; }
	public string? AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation { get; set; }
	public bool? AdmissionsVariationAdmissionsTeamSupportive { get; set; }
	public string? AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation { get; set; }
	public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendation { get; set; }
	public Recommendation? Recommendation { get; set; }
	public string? RecommendationMoreInformation { get; set; }

	public SignificantChangeTaskStatus GetEqualitiesTaskStatus()
	{
		if (EqualitiesImpactAssessmentCompleted is null && EqualitiesImpactIdentified is null)
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (EqualitiesImpactAssessmentCompleted.HasValue && EqualitiesImpactIdentified.HasValue)
		{
			return SignificantChangeTaskStatus.Completed;
		}

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetReligiousBodyConsultationTaskStatus()
	{
		if (TrustConsultedReligiousBodyAnswer is null
		    && ReligiousBodyRaisedObjection is null
		    && ReligiousBodyConsentGranted is null
		    && string.IsNullOrWhiteSpace(ReligiousBodySupportingEvidence))
			return SignificantChangeTaskStatus.NotStarted;

		if (TrustConsultedReligiousBodyAnswer.HasValue
		    && (TrustConsultedReligiousBodyAnswer is not YesNoNotApplicableAnswer.No || !string.IsNullOrWhiteSpace(TrustConsultedReligiousBodyNotConsultedReason))
		    && ReligiousBodyRaisedObjection.HasValue
		    && (ReligiousBodyRaisedObjection is not YesNoNotApplicableAnswer.Yes || !string.IsNullOrWhiteSpace(ReligiousBodyRaisedObjectionReason))
		    && ReligiousBodyConsentGranted.HasValue
		    && (ReligiousBodyConsentGranted is not YesNoNotApplicableAnswer.No || !string.IsNullOrWhiteSpace(ReligiousBodyConsentNotGrantedReason)))
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetOtherStakeholderEngagementTaskStatus()
	{
		if (TrustConsultedStakeholders is null
		    && WereThereAnyObjectionsRaised is null
		    && RelevantStakeholdersNotifiedOfAdmissionsChanges is null
		    && string.IsNullOrWhiteSpace(OtherStakeholderEngagementSupportingEvidence))
			return SignificantChangeTaskStatus.NotStarted;

		if (
			(TrustConsultedStakeholders is true || TrustConsultedStakeholders is false && !string.IsNullOrWhiteSpace(TrustConsultedStakeholdersNotConsultedReason))
			&& (WereThereAnyObjectionsRaised is false || WereThereAnyObjectionsRaised is true && !string.IsNullOrWhiteSpace(StakeholderObjectionsDetails))
			&& (RelevantStakeholdersNotifiedOfAdmissionsChanges is YesNoNotApplicableAnswer.Yes
				|| RelevantStakeholdersNotifiedOfAdmissionsChanges is YesNoNotApplicableAnswer.NotApplicable
				|| RelevantStakeholdersNotifiedOfAdmissionsChanges is YesNoNotApplicableAnswer.No && !string.IsNullOrWhiteSpace(RelevantStakeholdersNotifiedOfAdmissionsChangesReason))
		)
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetConfirmProjectDatesTaskStatus()
	{
		if (!ProposedDecisionDate.HasValue && !ProposedChangeDate.HasValue)
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (ProposedDecisionDate.HasValue && ProposedChangeDate.HasValue)
		{
			return SignificantChangeTaskStatus.Completed;
		}

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetConsultationDurationTaskStatus()
	{
		if (!ConsultationLastedMinimumThreeWeeks.HasValue
		    && string.IsNullOrWhiteSpace(ConsultationDurationNotMetReason))
			return SignificantChangeTaskStatus.NotStarted;

		if (ConsultationLastedMinimumThreeWeeks is ConsultationDurationAnswer.Yes
		    or ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)
			return SignificantChangeTaskStatus.Completed;

		if (ConsultationLastedMinimumThreeWeeks is ConsultationDurationAnswer.No
		    && !string.IsNullOrWhiteSpace(ConsultationDurationNotMetReason))
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetPlanningPermissionTaskStatus()
	{
		if (PlanningPermission is null)
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		return SignificantChangeTaskStatus.Completed;
	}

	public SignificantChangeTaskStatus GetAdmissionVariationTaskStatus()
	{
		if (AdmissionsVariationRequired is false) return SignificantChangeTaskStatus.Completed;
		if (
			AdmissionsVariationAdmissionsTeamSupportive is not null
			&& AdmissionsVariationLocalAuthoritySupportive is not null
			&& AdmissionsVariationPupilPlacePlanningSupportive is not null
		)
		{
			if (AdmissionsVariationLocalAuthoritySupportive is false
			    && string.IsNullOrWhiteSpace(AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation)
			    || AdmissionsVariationPupilPlacePlanningSupportive is false
			    && string.IsNullOrWhiteSpace(AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation)
			    || AdmissionsVariationAdmissionsTeamSupportive is false
			    && string.IsNullOrWhiteSpace(AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation))
			{
				return SignificantChangeTaskStatus.InProgress;
			}

			return SignificantChangeTaskStatus.Completed;
		}

		return SignificantChangeTaskStatus.NotStarted;
	}

	public SignificantChangeTaskStatus GetFundingTaskStatus()
	{
		if (!FundingAnswer.HasValue && string.IsNullOrWhiteSpace(FundingAdditionalInformation) &&
		    string.IsNullOrWhiteSpace(FundingSupportingEvidence))
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (FundingAnswer is SignificantChange.FundingAnswer.No &&
		    string.IsNullOrWhiteSpace(FundingAdditionalInformation))
		{
			return SignificantChangeTaskStatus.InProgress;
		}

		return FundingAnswer.HasValue
			? SignificantChangeTaskStatus.Completed
			: SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetRecommendationTaskStatus()
	{
		if (!Recommendation.HasValue && string.IsNullOrWhiteSpace(RecommendationMoreInformation))
			return SignificantChangeTaskStatus.NotStarted;

		if (Recommendation.HasValue)
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetHqtfConsiderationTaskStatus()
	{
		if (HqtfConsiderationAnswer is null)
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		return SignificantChangeTaskStatus.Completed;
	}

	public SignificantChangeTaskStatus GetOfstedTaskStatus()
	{
		if (OfstedInspectionDate is null)
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		return SignificantChangeTaskStatus.Completed;
	}

	public SignificantChangeTaskStatus GetFhaRisksOrIssuesRaisedTaskStatus()
	{
		return FhaRisksOrIssuesRaised.HasValue
			? SignificantChangeTaskStatus.Completed
			: SignificantChangeTaskStatus.NotStarted;
	}

	public SignificantChangeTaskStatus GetAdmissionsVariationRecommendationTaskStatus()
	{
		if (AdmissionsVariationRecommendation.HasValue)
		{
			return SignificantChangeTaskStatus.Completed;
		}

		return SignificantChangeTaskStatus.NotStarted;
	}
}
