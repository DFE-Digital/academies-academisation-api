namespace Dfe.Academies.Academisation.IService.ServiceModels;

public record ApplicationSchoolServiceModel (
	int Id,
	int Urn,
	string? ProposedNewSchoolName = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? SchoolCapacityAssumptions = null,
	int? SchoolCapacityPublishedAdmissionsNumber = null
);
