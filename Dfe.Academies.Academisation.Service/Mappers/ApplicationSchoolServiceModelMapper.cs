using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationSchoolServiceModelMapper
{
	internal static ApplicationSchoolServiceModel FromDomain(ISchool school)
	{
		return new(
			school.Id,
			school.Details.Urn,
			school.Details.SchoolName
		)
		{
			SchoolConversionApproverContactEmail = school.Details.ApproverContactEmail,
			SchoolConversionApproverContactName = school.Details.ApproverContactName,
			SchoolConversionMainContactOtherEmail = school.Details.MainContactOtherEmail,
			SchoolConversionMainContactOtherName = school.Details.MainContactOtherName,
			SchoolConversionMainContactOtherTelephone = school.Details.MainContactOtherTelephone,
			SchoolConversionMainContactOtherRole = school.Details.MainContactOtherRole,
			SchoolConversionContactHeadTel = school.Details.ContactHeadTel,
			SchoolConversionContactHeadName = school.Details.ContactHeadName,
			SchoolConversionContactHeadEmail = school.Details.ContactHeadEmail,
			SchoolConversionContactChairEmail = school.Details.ContactChairEmail,
			SchoolConversionContactChairName = school.Details.ContactChairName,
			SchoolConversionContactChairTel = school.Details.ContactChairTel,
			SchoolConversionContactRole = school.Details.ContactRole,
			ApplicationJoinTrustReason = school.Details.ApplicationJoinTrustReason,
			SchoolConversionTargetDate = school.Details.ConversionTargetDate,
			SchoolConversionTargetDateExplained = school.Details.ConversionTargetDateExplained,
			ProposedNewSchoolName = school.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = school.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = school.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = school.Details.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = school.Details.CapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = school.Details.CapacityPublishedAdmissionsNumber,
			PartOfPfiSchemeType = school.Details.PartOfPfiSchemeType,
			PartOfPfiScheme = school.Details.PartOfPfiScheme,
			PartOfBuildingSchoolsForFutureProgramme = school.Details.PartOfBuildingSchoolsForFutureProgramme,
			PartOfPrioritySchoolsBuildingProgramme = school.Details.PartOfPrioritySchoolsBuildingProgramme,
			WhichBodyAwardedGrants = school.Details.WhichBodyAwardedGrants,
			LbGrants = school.Details.LbGrants,
			LbWorksPlannedDate = school.Details.LbWorksPlannedDate,
			LbWorksPlannedExplained = school.Details.LbWorksPlannedExplained,
			LbWorksPlanned = school.Details.LbWorksPlanned,
			LbFacilitiesSharedExplained = school.Details.LbFacilitiesSharedExplained,
			LbFacilitiesShared = school.Details.LbFacilitiesShared,
			LbBuildLandOwnerExplained = school.Details.LbBuildLandOwnerExplained
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, serviceModel.SchoolName)
		{
			ApproverContactEmail = serviceModel.SchoolConversionApproverContactEmail,
			ApproverContactName = serviceModel.SchoolConversionApproverContactName,
			MainContactOtherTelephone = serviceModel.SchoolConversionMainContactOtherTelephone,
			MainContactOtherRole = serviceModel.SchoolConversionMainContactOtherRole,
			MainContactOtherName = serviceModel.SchoolConversionMainContactOtherName,
			MainContactOtherEmail = serviceModel.SchoolConversionMainContactOtherEmail,
			ContactHeadTel = serviceModel.SchoolConversionContactHeadTel,
			ContactHeadName = serviceModel.SchoolConversionContactHeadName,
			ContactHeadEmail = serviceModel.SchoolConversionContactHeadEmail,
			ContactChairEmail = serviceModel.SchoolConversionContactChairEmail,
			ContactChairName = serviceModel.SchoolConversionContactChairName,
			ContactChairTel = serviceModel.SchoolConversionContactChairTel,
			ContactRole = serviceModel.SchoolConversionContactRole,
			ApplicationJoinTrustReason = serviceModel.ApplicationJoinTrustReason,
			ConversionTargetDate = serviceModel.SchoolConversionTargetDate,
			ConversionTargetDateExplained = serviceModel.SchoolConversionTargetDateExplained,
			ProposedNewSchoolName = serviceModel.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = serviceModel.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = serviceModel.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = serviceModel.ProjectedPupilNumbersYear3,
			CapacityAssumptions = serviceModel.SchoolCapacityAssumptions,
			CapacityPublishedAdmissionsNumber = serviceModel.SchoolCapacityPublishedAdmissionsNumber,
			PartOfPfiSchemeType = serviceModel.PartOfPfiSchemeType,
			PartOfPfiScheme = serviceModel.PartOfPfiScheme,
			PartOfBuildingSchoolsForFutureProgramme = serviceModel.PartOfBuildingSchoolsForFutureProgramme,
			PartOfPrioritySchoolsBuildingProgramme = serviceModel.PartOfPrioritySchoolsBuildingProgramme,
			WhichBodyAwardedGrants = serviceModel.WhichBodyAwardedGrants,
			LbGrants = serviceModel.LbGrants,
			LbWorksPlannedDate = serviceModel.LbWorksPlannedDate,
			LbWorksPlannedExplained = serviceModel.LbWorksPlannedExplained,
			LbWorksPlanned = serviceModel.LbWorksPlanned,
			LbFacilitiesSharedExplained = serviceModel.LbFacilitiesSharedExplained,
			LbFacilitiesShared = serviceModel.LbFacilitiesShared,
			LbBuildLandOwnerExplained = serviceModel.LbBuildLandOwnerExplained
		};
	}
}
