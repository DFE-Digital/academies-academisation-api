using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionGetDataByProjectIdQuery : IAdvisoryBoardDecisionGetDataByProjectIdQuery
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

	public AdvisoryBoardDecisionGetDataByProjectIdQuery(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
	}

	public async Task<IConversionAdvisoryBoardDecision?> Execute(int projectId, bool isTransfer = false)
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

		return decision;
	}

	public async Task<IConversionAdvisoryBoardDecision?> Execute(List<int?> projectIds, bool isTransfer = false)
	{
		List<AdvisoryBoardDecisionState> state = new List<AdvisoryBoardDecisionState>();

		var advisoryBoardDecisions = _context.ConversionAdvisoryBoardDecisions
				.Include(s => s.DeclinedReasons)
				.Include(s => s.DeferredReasons)
				.Include(s => s.WithdrawnReasons);

		if (isTransfer)
		{

			//.Where(a => projectIds.Contains(a.TransferProjectId));

			//state = advisoryBoardDecisions.Where(s => projectIds.Contains(s.TransferProjectId)).Select(r => r.MapToDomain());
				
				
				
				//.Select(r => r.MapToDomain()).ToList();
		}
		else
		{
			//state = await advisoryBoardDecisions.Where(s => s.ConversionProjectId == projectId);
		}

		//TODO:EA finish this method
		return null;
	}
}
