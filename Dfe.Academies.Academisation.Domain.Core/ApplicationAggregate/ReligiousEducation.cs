namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record ReligiousEducation
	(
		// TODO MR:- props as per V2 specification - part of additional info legacy model
		bool? SchoolFaithSchool = null,
		string? SchoolFaithSchoolDioceseName = null,
		string? DiocesePermissionEvidenceDocumentLink = null,
		bool? SchoolHasSACREException = null,
		DateTime? SchoolSACREExemptionEndDate = null
	);
}
