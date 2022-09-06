namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public record SchoolDetails(
	int Urn,
	string SchoolName,
	LandAndBuildings LandAndBuildings,
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
	bool? SchoolConversionTargetDateSpecified = null,
	DateTime? ConversionTargetDate = null,
	string? ConversionTargetDateExplained = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? CapacityAssumptions = null,
	int? CapacityPublishedAdmissionsNumber = null
);
