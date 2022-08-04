using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplyingSchoolServiceModelMapper
{
	internal static ApplyingSchoolServiceModel FromDomain(IApplyingSchool school)
	{
		return new(
			school.Id,
			school.Details.Urn
		);
	} 
}
