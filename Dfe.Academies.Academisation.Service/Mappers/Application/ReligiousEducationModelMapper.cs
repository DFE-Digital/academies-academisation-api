using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ReligiousEducationModelMapper
{
	internal static ReligiousEducationServiceModel ToServiceModel(this ReligiousEducation religiousEducation)
	{
		//TODO MR:-
		//		bool? SchoolFaithSchool = null, // linked to a diocese yes/no?
		//string? SchoolFaithSchoolDioceseName = null,
		//string? DiocesePermissionEvidenceDocumentLink = null,
		//bool? SchoolHasSACREException = null,
		//	DateTime? SchoolSACREExemptionEndDate = null	
	}

	internal static ReligiousEducation ToDomain(this ReligiousEducationServiceModel serviceModel)
	{
		//TODO MR:-
	}
}
