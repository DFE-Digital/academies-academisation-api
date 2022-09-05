using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.LegalRequirement;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;

namespace Dfe.Academies.Academisation.Service.Commands.LegalRequirements
{
	public class LegalRequirementCreateCommand : ILegalRequirementCreateCommand
	{
		public Task<CreateResult<LegalRequirementServiceModel>> Execute(LegalRequirementServiceModel requestModel)
		{
			throw new NotImplementedException();
		}
	}
}
