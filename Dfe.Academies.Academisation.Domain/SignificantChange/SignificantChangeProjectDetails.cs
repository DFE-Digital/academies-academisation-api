namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public bool? TrustConsultedReligiousBody { get; set; }
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }

	public bool? EqualitiesImpactAssessmentCompleted { get; set; }
	public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
	public string? EqualitiesImpactIdentifiedMitigation { get; set; }
  
  public bool? ConsultationIncludeAdmissionVariation { get; set; }
	public string? ConsultationNoAdmissionVariationReason { get; set; }

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
  
  
}
