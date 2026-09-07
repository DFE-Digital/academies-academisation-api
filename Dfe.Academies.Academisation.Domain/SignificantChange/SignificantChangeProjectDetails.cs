namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }

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
}
