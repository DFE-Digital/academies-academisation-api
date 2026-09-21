using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeConsultationDurationResponse
{
	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }
	public string Status { get; set; } = string.Empty;
}