namespace Dfe.Academies.Academisation.Domain.SignificantChange;

public class SignificantChangeProjectDetails
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }

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

	public bool? ConsultationIncludeAdmissionVariation { get; set; }
	public bool? ConsultationIncludeAdmissionVariationNotApplicable { get; set; }
	public string? ConsultationNoAdmissionVariationReason { get; set; }

	public SignificantChangeTaskStatus GetAdmissionVariationConsultationTaskStatus()
	{
		if (!ConsultationIncludeAdmissionVariation.HasValue
		    && !ConsultationIncludeAdmissionVariationNotApplicable.HasValue
		    && string.IsNullOrWhiteSpace(ConsultationNoAdmissionVariationReason))
		{
			return SignificantChangeTaskStatus.NotStarted;
		}

		if (ConsultationIncludeAdmissionVariationNotApplicable is true)
		{
			return SignificantChangeTaskStatus.NoApplicable;
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
