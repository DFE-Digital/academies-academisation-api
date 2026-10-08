
namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class EqualitiesImpactAssessmentResponse
	{
        public bool? EqualitiesImpactAssessmentCompleted { get; set; }
        public string? EqualitiesImpactIdentified { get; set; }
        public string? EqualitiesLikelyDetails { get; set; }
        public string? EqualitiesSomeImpactDetails { get; set; }
		public string? EqualitiesImpactSupportingEvidence { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
