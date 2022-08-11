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
			ProposedNewSchoolName = school.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = school.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = school.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = school.Details.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = school.Details.SchoolCapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = school.Details.SchoolCapacityPublishedAdmissionsNumber
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, serviceModel.SchoolName)
		{
			SchoolConversionApproverContactEmail = serviceModel.SchoolConversionApproverContactEmail,
			SchoolConversionApproverContactName = serviceModel.SchoolConversionApproverContactName,
			SchoolConversionMainContactOtherTelephone = serviceModel.SchoolConversionMainContactOtherTelephone,
			SchoolConversionMainContactOtherRole = serviceModel.SchoolConversionMainContactOtherRole,
			SchoolConversionMainContactOtherName = serviceModel.SchoolConversionMainContactOtherName,
			SchoolConversionMainContactOtherEmail = serviceModel.SchoolConversionMainContactOtherEmail,
			SchoolConversionContactHeadTel = serviceModel.SchoolConversionContactHeadTel,
			SchoolConversionContactHeadName = serviceModel.SchoolConversionContactHeadName,
			SchoolConversionContactHeadEmail = serviceModel.SchoolConversionContactHeadEmail,
			SchoolConversionContactChairEmail = serviceModel.SchoolConversionContactChairEmail,
			SchoolConversionContactChairName = serviceModel.SchoolConversionContactChairName,
			SchoolConversionContactChairTel = serviceModel.SchoolConversionContactChairTel,
			SchoolConversionContactRole = serviceModel.SchoolConversionContactRole,
			ApplicationJoinTrustReason = serviceModel.ApplicationJoinTrustReason,
			SchoolConversionTargetDate =	serviceModel.SchoolConversionTargetDate,
			SchoolConversionTargetDateExplained = serviceModel.SchoolConversionTargetDateExplained,
			ProposedNewSchoolName = serviceModel.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = serviceModel.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = serviceModel.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = serviceModel.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = serviceModel.SchoolCapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = serviceModel.SchoolCapacityPublishedAdmissionsNumber
		};
	}
}
