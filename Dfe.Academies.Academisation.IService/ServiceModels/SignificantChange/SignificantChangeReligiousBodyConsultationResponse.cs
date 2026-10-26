using Newtonsoft.Json;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeReligiousBodyConsultationResponse
{
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public int? TrustConsultedReligiousBody { get; set; }
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public int? ReligiousBodyRaisedObjection { get; set; }
	public string? ReligiousBodyRaisedObjectionReason { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public int? ReligiousBodyConsentGranted { get; set; }
	public string? ReligiousBodyConsentNotGrantedReason { get; set; }
	public string? SupportingEvidence { get; set; }
	public int Status { get; set; }
}