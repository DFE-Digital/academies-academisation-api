namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }

	public bool? EqualitiesImpactAssessmentCompleted { get; set; }
	public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
	public string? EqualitiesImpactIdentifiedMitigation { get; set; }

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
}
