namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public record LandAndBuildings(
	string? OwnerExplained = null,
	bool? WorksPlanned = null,
	DateTime? WorksPlannedDate = null,
	string? WorksPlannedExplained = null,
	bool? FacilitiesShared = null,
	string? FacilitiesSharedExplained = null,
	bool? Grants = null,
	string? GrantsAwardingBodies = null,
	bool? PartOfPfiScheme = null,
	string? PartOfPfiSchemeType = null,
	bool? PartOfPrioritySchoolsBuildingProgramme = null,
	bool? PartOfBuildingSchoolsForFutureProgramme = null
);
