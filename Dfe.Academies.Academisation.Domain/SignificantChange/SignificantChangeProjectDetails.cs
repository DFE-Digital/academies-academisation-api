namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }	
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
}