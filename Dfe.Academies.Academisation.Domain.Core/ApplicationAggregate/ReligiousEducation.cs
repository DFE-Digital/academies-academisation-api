namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record ReligiousEducation
	(
		bool? FaithSchool = null, // linked to a diocese yes/no?
		string? FaithSchoolDioceseName = null,
		string? DiocesePermissionEvidenceDocumentLink = null,
		bool? HasSACREException = null,
		DateTime? SACREExemptionEndDate = null
	);
}
