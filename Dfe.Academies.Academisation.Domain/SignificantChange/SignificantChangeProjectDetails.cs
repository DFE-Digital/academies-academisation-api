namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }

	public SignificantChangeTaskStatus GetConsultStakeholdersTaskStatus()
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
}