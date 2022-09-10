namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record ReligiousEducationServiceModel(
		bool? FaithSchool = null, // linked to a diocese yes/no?
		string? FaithSchoolDioceseName = null,
		string? DiocesePermissionEvidenceDocumentLink = null,
		bool? HasSACREException = null,
		DateTime? SACREExemptionEndDate = null
		);
}
