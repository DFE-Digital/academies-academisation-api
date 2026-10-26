using Dfe.Academies.Academisation.Domain.SignificantChange;
using Newtonsoft.Json;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeConsultationDurationResponse
{
	[JsonProperty(NullValueHandling = NullValueHandling.Include)]
	public int? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }
	public int Status { get; set; }
}