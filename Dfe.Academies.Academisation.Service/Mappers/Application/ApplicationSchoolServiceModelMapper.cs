﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ApplicationSchoolServiceModelMapper
{
	internal static ApplicationSchoolServiceModel FromDomain(ISchool school)
	{
		return new(
			school.Id,
			school.Details.Urn,
			school.Details.SchoolName,
			school.Details.LandAndBuildings.ToServiceModel()
		)
		{
			// TODO MR:- additional information other:-
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
			SchoolConversionTargetDateSpecified = school.Details.ConversionTargetDateSpecified,
			SchoolConversionTargetDate = school.Details.ConversionTargetDate,
			SchoolConversionTargetDateExplained = school.Details.ConversionTargetDateExplained,
			ConversionChangeNamePlanned = school.Details.ConversionChangeNamePlanned,
			ProposedNewSchoolName = school.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = school.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = school.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = school.Details.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = school.Details.CapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = school.Details.CapacityPublishedAdmissionsNumber,
			SchoolSupportGrantFundsPaidTo = school.Details.SchoolSupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = school.Details.ConfirmPaySupportGrantToSchool,
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, serviceModel.SchoolName, serviceModel.LandAndBuildings.ToDomain())
		{
			// TODO MR:- additional information other:-
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
			ConversionTargetDateSpecified = serviceModel.SchoolConversionTargetDateSpecified,
			ConversionTargetDate = serviceModel.SchoolConversionTargetDate,
			ConversionTargetDateExplained = serviceModel.SchoolConversionTargetDateExplained,
			ConversionChangeNamePlanned = serviceModel.ConversionChangeNamePlanned,
			ProposedNewSchoolName = serviceModel.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = serviceModel.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = serviceModel.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = serviceModel.ProjectedPupilNumbersYear3,
			CapacityAssumptions = serviceModel.SchoolCapacityAssumptions,
			CapacityPublishedAdmissionsNumber = serviceModel.SchoolCapacityPublishedAdmissionsNumber,
			SchoolSupportGrantFundsPaidTo = serviceModel.SchoolSupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = serviceModel.ConfirmPaySupportGrantToSchool,
		};
	}
}
