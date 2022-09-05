using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.IService.Query;

public interface IConversionAdvisoryBoardDecisionGetQuery
{
	Task<ConversionAdvisoryBoardDecisionServiceModel?> Execute(int projectId);
}
