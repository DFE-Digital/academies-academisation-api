namespace Dfe.Academies.Academisation.IService.ServiceModels;

public record ApplicationSchoolServiceModel(
	int Id,
	int Urn,
	string SchoolName,
	// contact details
	string? SchoolConversionContactHeadName = null,
	string? SchoolConversionContactHeadEmail = null,
	string? SchoolConversionContactHeadTel = null,
	string? SchoolConversionContactChairName = null,
	string? SchoolConversionContactChairEmail = null,
	string? SchoolConversionContactChairTel = null,
	string? SchoolConversionContactRole = null, // "headteacher", "chair of governing body", "someone else"
	string? SchoolConversionMainContactOtherName = null,
	string? SchoolConversionMainContactOtherEmail = null,
	string? SchoolConversionMainContactOtherTelephone = null,
	string? SchoolConversionMainContactOtherRole = null,
	string? SchoolConversionApproverContactName = null,
	string? SchoolConversionApproverContactEmail = null,
	// conversion details
	DateTime? SchoolConversionTargetDate = null,
	string? SchoolConversionTargetDateExplained = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? SchoolCapacityAssumptions = null,
	int? SchoolCapacityPublishedAdmissionsNumber = null,
	// land and buildings
	string? PartOfPfiSchemeType = null,
	int? PartOfPfiScheme = null,
	int? PartOfBuildingSchoolsForFutureProgramme = null,
	int? PartOfPrioritySchoolsBuildingProgramme = null,
	string? WhichBodyAwardedGrants = null,
	int? LbGrants = null,
	DateTime? LbWorksPlannedDate = null,
	string? LbWorksPlannedExplained = null,
	int? LbWorksPlanned = null,
	string? LbFacilitiesSharedExplained = null,
	int? LbFacilitiesShared = null,
	string? LbBuildLandOwnerExplained = null
);
