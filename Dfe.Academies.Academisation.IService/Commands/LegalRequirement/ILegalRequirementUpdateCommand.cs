using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;

namespace Dfe.Academies.Academisation.IService.Commands.LegalRequirement
{
	public interface ILegalRequirementUpdateCommand
	{
		Task<CommandResult> Execute(LegalRequirementServiceModel requestModel);
	}
}
