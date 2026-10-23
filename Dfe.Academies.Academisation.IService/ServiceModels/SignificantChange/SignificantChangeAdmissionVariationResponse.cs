
namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange
{
	public class SignificantChangeAdmissionVariationResponse
	{
		public bool? AdmissionsVariationRequired { get; set; }
		public string? AdmissionsVariationSupportingEvidence { get; set; }
		public bool? AdmissionsVariationLocalAuthoritySupportive { get; set; }
		public string? AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation { get; set; }
		public bool? AdmissionsVariationPupilPlacePlanningSupportive { get; set; }
		public string? AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation { get; set; }
		public bool? AdmissionsVariationAdmissionsTeamSupportive { get; set; }
		public string? AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
