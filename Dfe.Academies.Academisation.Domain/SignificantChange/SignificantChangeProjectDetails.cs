namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public bool? TrustConsultedReligiousBody { get; set; }
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }
	public SignificantChangeStakeholderObjections? StakeholderObjections { get; set; }
	public string? StakeholderObjectionsComment { get; set; }
	public bool? EqualitiesImpactAssessmentCompleted { get; set; }
	public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
	public string? EqualitiesImpactIdentifiedMitigation { get; set; }

	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }
	public bool? ConsultationIncludeAdmissionVariation { get; set; }
	public string? ConsultationNoAdmissionVariationReason { get; set; }
	public FundingAnswer? FundingAnswer { get; set; }
	public string? FundingAdditionalInformation { get; set; }
	public string? FundingSupportingEvidence { get; set; }
	public PlanningPermissionAnswer? PlanningPermission { get; set; }
	public string? PlanningPermissionSupportingEvidence { get; set; }
	public string? PlanningPermissionAdditionalInformation { get; set; }

	public SignificantChange_Generic_YesNoNa? LandTransactionConsent { get; set; }
	public string? LandTransactionConsentAdditionalInfo { get; set; }
	public SignificantChange_Generic_YesNoNa? LandTransactionApplication { get; set; }
	public string? LandTransactionApplicationAdditionalInfo { get; set; }
	public string? LandTransactionSupportingEvidence { get; set; }

	public SignificantChangeTaskStatus GetStakeholderConsultationTaskStatus()
	{
		if (!TrustConsultedStakeholders.HasValue
		    && string.IsNullOrWhiteSpace(TrustConsultedStakeholdersNotConsultedReason))
			return SignificantChangeTaskStatus.NotStarted;

		if (TrustConsultedStakeholders is true)
			return SignificantChangeTaskStatus.Completed;

		if (TrustConsultedStakeholders is false
		    && !string.IsNullOrWhiteSpace(TrustConsultedStakeholdersNotConsultedReason))
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}

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
		if (!TrustConsultedReligiousBody.HasValue
		    && string.IsNullOrWhiteSpace(TrustConsultedReligiousBodyNotConsultedReason))
			return SignificantChangeTaskStatus.NotStarted;

		if (TrustConsultedReligiousBody is true)
			return SignificantChangeTaskStatus.Completed;

		if (TrustConsultedReligiousBody is false
		    && !string.IsNullOrWhiteSpace(TrustConsultedReligiousBodyNotConsultedReason))
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

	public SignificantChangeTaskStatus GetLandTransactionTaskStatus()
	{
		if (!LandTransactionApplication.HasValue
			&& string.IsNullOrWhiteSpace(LandTransactionApplicationAdditionalInfo)
			&& !LandTransactionConsent.HasValue
			&& string.IsNullOrWhiteSpace(LandTransactionConsentAdditionalInfo)
			&& string.IsNullOrWhiteSpace(LandTransactionSupportingEvidence) )
			return SignificantChangeTaskStatus.NotStarted;

		if (LandTransactionApplication.HasValue
			&& (LandTransactionApplication != SignificantChange_Generic_YesNoNa.No || !string.IsNullOrWhiteSpace(LandTransactionApplicationAdditionalInfo))
			&& LandTransactionConsent.HasValue
			&& (LandTransactionConsent != SignificantChange_Generic_YesNoNa.No || !string.IsNullOrWhiteSpace(LandTransactionConsentAdditionalInfo)))
			return SignificantChangeTaskStatus.Completed;

		return SignificantChangeTaskStatus.InProgress;
	}
	public SignificantChangeTaskStatus GetStakeholderObjectionsTaskStatus()
	{
		if (!StakeholderObjections.HasValue
		    && string.IsNullOrWhiteSpace(StakeholderObjectionsComment))
			return SignificantChangeTaskStatus.NotStarted;

		if (StakeholderObjections == SignificantChangeStakeholderObjections.YesAllObjectionsAddressed)
			return SignificantChangeTaskStatus.Completed;

		if (StakeholderObjections == SignificantChangeStakeholderObjections.No)
			return SignificantChangeTaskStatus.Completed;

		if (StakeholderObjections == SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided
		    && !string.IsNullOrWhiteSpace(StakeholderObjectionsComment))
			return SignificantChangeTaskStatus.Completed;

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

	public SignificantChangeTaskStatus GetAdmissionVariationConsultationTaskStatus()
	{
		if (!ConsultationIncludeAdmissionVariation.HasValue
		    && string.IsNullOrWhiteSpace(ConsultationNoAdmissionVariationReason))
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (ConsultationIncludeAdmissionVariation is true)
		{
			return SignificantChangeTaskStatus.Completed;
		}

		if (ConsultationIncludeAdmissionVariation is false
		    && !string.IsNullOrWhiteSpace(ConsultationNoAdmissionVariationReason))
		{
			return SignificantChangeTaskStatus.Completed;
		}

		return SignificantChangeTaskStatus.InProgress;
	}

	public SignificantChangeTaskStatus GetFundingTaskStatus()
	{
		if (!FundingAnswer.HasValue && string.IsNullOrWhiteSpace(FundingAdditionalInformation) && string.IsNullOrWhiteSpace(FundingSupportingEvidence))
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (FundingAnswer is SignificantChange.FundingAnswer.No && string.IsNullOrWhiteSpace(FundingAdditionalInformation))
		{
			return SignificantChangeTaskStatus.InProgress;
		}

		return FundingAnswer.HasValue
			? SignificantChangeTaskStatus.Completed
			: SignificantChangeTaskStatus.InProgress;
	}
}
