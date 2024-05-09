using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.IService.Query;

public interface IAdvisoryBoardDecisionQueryService
{
	Task<ConversionAdvisoryBoardDecisionServiceModel?> GetByProjectId(int projectId, bool isTransfer = false);
}
