namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public class LandAndBuildings
{
	public LandAndBuildings(string? OwnerExplained = null,
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
		bool? PartOfBuildingSchoolsForFutureProgramme = null)
	{
		this.OwnerExplained = OwnerExplained;
		this.WorksPlanned = WorksPlanned;
		this.WorksPlannedDate = WorksPlannedDate;
		this.WorksPlannedExplained = WorksPlannedExplained;
		this.FacilitiesShared = FacilitiesShared;
		this.FacilitiesSharedExplained = FacilitiesSharedExplained;
		this.Grants = Grants;
		this.GrantsAwardingBodies = GrantsAwardingBodies;
		this.PartOfPfiScheme = PartOfPfiScheme;
		this.PartOfPfiSchemeType = PartOfPfiSchemeType;
		this.PartOfPrioritySchoolsBuildingProgramme = PartOfPrioritySchoolsBuildingProgramme;
		this.PartOfBuildingSchoolsForFutureProgramme = PartOfBuildingSchoolsForFutureProgramme;
	}

	public string? OwnerExplained { get; init; }
	public bool? WorksPlanned { get; init; }
	public DateTime? WorksPlannedDate { get; init; }
	public string? WorksPlannedExplained { get; init; }
	public bool? FacilitiesShared { get; init; }
	public string? FacilitiesSharedExplained { get; init; }
	public bool? Grants { get; init; }
	public string? GrantsAwardingBodies { get; init; }
	public bool? PartOfPfiScheme { get; init; }
	public string? PartOfPfiSchemeType { get; init; }
	public bool? PartOfPrioritySchoolsBuildingProgramme { get; init; }
	public bool? PartOfBuildingSchoolsForFutureProgramme { get; init; }

	public void Deconstruct(out string? OwnerExplained, out bool? WorksPlanned, out DateTime? WorksPlannedDate, out string? WorksPlannedExplained, out bool? FacilitiesShared, out string? FacilitiesSharedExplained, out bool? Grants, out string? GrantsAwardingBodies, out bool? PartOfPfiScheme, out string? PartOfPfiSchemeType, out bool? PartOfPrioritySchoolsBuildingProgramme, out bool? PartOfBuildingSchoolsForFutureProgramme)
	{
		OwnerExplained = this.OwnerExplained;
		WorksPlanned = this.WorksPlanned;
		WorksPlannedDate = this.WorksPlannedDate;
		WorksPlannedExplained = this.WorksPlannedExplained;
		FacilitiesShared = this.FacilitiesShared;
		FacilitiesSharedExplained = this.FacilitiesSharedExplained;
		Grants = this.Grants;
		GrantsAwardingBodies = this.GrantsAwardingBodies;
		PartOfPfiScheme = this.PartOfPfiScheme;
		PartOfPfiSchemeType = this.PartOfPfiSchemeType;
		PartOfPrioritySchoolsBuildingProgramme = this.PartOfPrioritySchoolsBuildingProgramme;
		PartOfBuildingSchoolsForFutureProgramme = this.PartOfBuildingSchoolsForFutureProgramme;
	}
}
