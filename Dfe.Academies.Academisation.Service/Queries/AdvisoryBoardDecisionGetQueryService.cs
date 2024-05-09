using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Queries;

public class AdvisoryBoardDecisionGetQueryService : IAdvisoryBoardDecisionQueryService
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

	public AdvisoryBoardDecisionGetQueryService(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
	}

	public async Task<ConversionAdvisoryBoardDecisionServiceModel?> GetByProjectId(int projectId, bool isTransfer = false)
	{
		ConversionAdvisoryBoardDecision? decision = null;

		if (isTransfer)
		{
			decision = await _advisoryBoardDecisionRepository.GetTransferProjectDecsion(projectId);
		}
		else
		{
			decision = await _advisoryBoardDecisionRepository.GetConversionProjectDecsion(projectId);
		}

		return decision?.MapFromDomain();
	}
}
