using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService.Commands;

public interface IAdvisoryBoardDecisionUpdateCommand
{
	Task<CommandResult> Execute(ConversionAdvisoryBoardDecisionServiceModel requestModel);
}
