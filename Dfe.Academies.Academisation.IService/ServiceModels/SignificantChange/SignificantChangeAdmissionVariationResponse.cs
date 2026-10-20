
namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeAdmissionVariationResponse
	{
		public bool? AdmissionsVariationRequired { get; set; }
		public string? AdmissionsVariationSupportingEvidence { get; set; }
		public bool? AdmissionsVariationLocalAuthoritySupportive { get; set; }
		public bool? AdmissionsVariationPupilPlacePlanningSupportive { get; set; }
		public bool? AdmissionsVariationAdmissionsTeamSupportive { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
