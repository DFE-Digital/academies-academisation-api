using Dfe.Academies.Academisation.Domain.Core.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeRecommendationResponse
	{
		public Recommendation Recommendation { get; set; }
		public string? RecommendationMoreInformation { get; set; } = null;
	}
}
