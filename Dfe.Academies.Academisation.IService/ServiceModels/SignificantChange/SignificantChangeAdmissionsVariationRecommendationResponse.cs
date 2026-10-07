
using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeAdmissionsVariationRecommendationResponse
	{
		public AdmissionsVariationRecommendationAnswer AdmissionsVariationRecommendation { get; set; }
		public string? AdditionalInformation { get; set; } = null;
		public string Status { get; set; } = string.Empty;
	}
}
