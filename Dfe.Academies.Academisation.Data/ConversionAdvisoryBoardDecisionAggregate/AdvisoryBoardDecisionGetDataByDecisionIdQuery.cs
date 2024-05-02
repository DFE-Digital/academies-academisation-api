using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionGetDataByDecisionIdQuery : IAdvisoryBoardDecisionGetDataByDecisionIdQuery
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

	public AdvisoryBoardDecisionGetDataByDecisionIdQuery(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
	}

	public async Task<IConversionAdvisoryBoardDecision?> Execute(int decisionId)
	{
		var decision = await _advisoryBoardDecisionRepository.GetAdvisoryBoardDecisionById(decisionId);

		return decision;
	}
}
