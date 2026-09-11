using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Queries;

public class AdvisoryBoardDecisionGetQueryService(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	: IAdvisoryBoardDecisionQueryService
{
	public async Task<ConversionAdvisoryBoardDecisionServiceModel?> GetByProjectId(int projectId, bool isTransfer = false)
	{
		ConversionAdvisoryBoardDecision? decision;

		if (isTransfer)
		{
			decision = await advisoryBoardDecisionRepository.GetTransferProjectDecision(projectId);
		}
		else
		{
			decision = await advisoryBoardDecisionRepository.GetConversionProjectDecision(projectId);
		}

		return decision?.MapFromDomain();
	}
}
