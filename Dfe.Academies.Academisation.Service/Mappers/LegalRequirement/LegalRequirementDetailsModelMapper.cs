using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;

namespace Dfe.Academies.Academisation.Service.Mappers.LegalRequirement
{
	public static class LegalRequirementDetailsModelMapper
	{
		public static LegalRequirementServiceModel MapFromDomain(
		this IDomain.LegalRequirementAggregate.LegalRequirement legalRequirement)
		{
			return new()
			{
				Id = legalRequirement.Id,
				ProjectId = legalRequirement.LegalRequirementDetails.ProjectId,
				HadConsultation = legalRequirement.LegalRequirementDetails.HadConsultation,
				HasDioceseConsented = legalRequirement.LegalRequirementDetails.HasDioceseConsented,
				HasFoundationConsented = legalRequirement.LegalRequirementDetails.HasFoundationConsented,
				HaveProvidedResolution = legalRequirement.LegalRequirementDetails.HaveProvidedResolution,				
				IsSectionComplete = legalRequirement.LegalRequirementDetails.IsSectionComplete				
			};
		}	
	}
}
