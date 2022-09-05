using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;

public interface IAdvisoryBoardDecisionUpdateCommand
{
	Task<CommandResult> Execute(ConversionAdvisoryBoardDecisionServiceModel requestModel);
}
