using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ReligiousEducationModelMapper
{
	internal static ReligiousEducationServiceModel ToServiceModel(this ReligiousEducation religiousEducation)
	{
		return new ReligiousEducationServiceModel
		{
			FaithSchool = religiousEducation.FaithSchool,
			FaithSchoolDioceseName = religiousEducation.FaithSchoolDioceseName,
			DiocesePermissionEvidenceDocumentLink = religiousEducation.DiocesePermissionEvidenceDocumentLink,
			HasSACREException = religiousEducation.HasSACREException,
			SACREExemptionEndDate = religiousEducation.SACREExemptionEndDate
		};
	}

	internal static ReligiousEducation ToDomain(this ReligiousEducationServiceModel serviceModel)
	{
		return new ReligiousEducation
		{
			FaithSchool = serviceModel.FaithSchool,
			FaithSchoolDioceseName = serviceModel.FaithSchoolDioceseName,
			DiocesePermissionEvidenceDocumentLink = serviceModel.DiocesePermissionEvidenceDocumentLink,
			HasSACREException = serviceModel.HasSACREException,
			SACREExemptionEndDate = serviceModel.SACREExemptionEndDate
		};
	}
}
