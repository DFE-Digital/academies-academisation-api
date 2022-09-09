﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record ApplicationSchoolServiceModel(
	int Id,
	int Urn,
	string SchoolName,
	LandAndBuildingsServiceModel LandAndBuildings,
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
	bool? SchoolConversionTargetDateSpecified = null,
	DateTime? SchoolConversionTargetDate = null,
	string? SchoolConversionTargetDateExplained = null,
	bool? ConversionChangeNamePlanned = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? SchoolCapacityAssumptions = null,
	int? SchoolCapacityPublishedAdmissionsNumber = null,
	// application pre-support grant
	PayFundsTo? SchoolSupportGrantFundsPaidTo = null,
	bool? ConfirmPaySupportGrantToSchool = null
);
