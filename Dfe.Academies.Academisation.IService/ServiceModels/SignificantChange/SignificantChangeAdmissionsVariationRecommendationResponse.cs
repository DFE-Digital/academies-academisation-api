
using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeAdmissionsVariationRecommendationResponse
	{
		public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendationAnswer { get; set; }
		public string? FurtherInformation { get; set; } = null;
		public string Status { get; set; } = string.Empty;
	}
}
