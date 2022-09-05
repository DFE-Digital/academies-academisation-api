using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ILegalRequirementGetQuery
	{
		Task<LegalRequirementServiceModel?> Execute(int projectId);
	}
}
