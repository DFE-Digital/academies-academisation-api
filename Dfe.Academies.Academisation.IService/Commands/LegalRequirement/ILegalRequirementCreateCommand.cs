using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;

namespace Dfe.Academies.Academisation.IService.Commands.LegalRequirement
{
	public interface ILegalRequirementCreateCommand
	{
		Task<CreateResult<LegalRequirementServiceModel>> Execute(LegalRequirementServiceModel requestModel);
	}
}
