
namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeAdmissionVariationConsultationResponse
	{
		public bool? ConsultationIncludeAdmissionVariation { get; set; }
		public bool? ConsultationIncludeAdmissionVariationNotApplicable { get; set; }
		public string? ConsultationNoAdmissionVariationReason { get; set; }

		public string Status { get; set; } = string.Empty;
	}
}
