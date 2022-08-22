using System.Text.Json.Serialization;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public record SchoolDetails(
	int Urn,
	string SchoolName,
	// contact details
	string? ContactHeadName = null,
	string? ContactHeadEmail = null,
	string? ContactHeadTel = null,
	string? ContactChairName = null,
	string? ContactChairEmail = null,
	string? ContactChairTel = null,
	string? ContactRole = null, // "headteacher", "chair of governing body", "someone else"
	string? MainContactOtherName = null,
	string? MainContactOtherEmail = null,
	string? MainContactOtherTelephone = null,
	string? MainContactOtherRole = null,
	string? ApproverContactName = null,
	string? ApproverContactEmail = null,
	// conversion details
	DateTime? ConversionTargetDate = null,
	string? ConversionTargetDateExplained = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? CapacityAssumptions = null,
	int? CapacityPublishedAdmissionsNumber = null,
	// land and buildings
	string? PartOfPfiSchemeType  = null,
	int? PartOfPfiScheme  = null,
	int? PartOfBuildingSchoolsForFutureProgramme  = null,
	int? PartOfPrioritySchoolsBuildingProgramme  = null,
	string? WhichBodyAwardedGrants  = null,
	int? LbGrants  = null,
	DateTime? LbWorksPlannedDate  = null,
	string? LbWorksPlannedExplained  = null,
	int? LbWorksPlanned  = null,
	string? LbFacilitiesSharedExplained  = null,
	int? LbFacilitiesShared  = null,
	string? LbBuildLandOwnerExplained  = null
);
