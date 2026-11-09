using Newtonsoft.Json;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeOtherStakeholderEngagementResponse
{
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public bool? WereThereAnyObjectionsRaised { get; set; }
	public string? StakeholderObjectionsDetails { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public int? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; }
	public string? RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; }
	public string? SupportingEvidence { get; set; }
	public string Status { get; set; } = string.Empty;
}
