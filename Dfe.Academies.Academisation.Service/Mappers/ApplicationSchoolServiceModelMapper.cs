﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationSchoolServiceModelMapper
{
	internal static ApplicationSchoolServiceModel FromDomain(ISchool school)
	{
		return new(
			school.Id,
			school.Details.Urn
		);
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(
			serviceModel.Urn,
			serviceModel.ProposedNewSchoolName,
			serviceModel.ProjectedPupilNumbersYear1,
			serviceModel.ProjectedPupilNumbersYear2,
			serviceModel.ProjectedPupilNumbersYear3,
			serviceModel.SchoolCapacityAssumptions,
			serviceModel.SchoolCapacityPublishedAdmissionsNumber
		);
	}
}
