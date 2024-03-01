using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionUpdateDataCommand : IAdvisoryBoardDecisionUpdateDataCommand
{
	private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

	public AdvisoryBoardDecisionUpdateDataCommand(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
	{
		_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
	}

	public async Task Execute(IConversionAdvisoryBoardDecision decision)
	{
		var x = decision as ConversionAdvisoryBoardDecision;

		_advisoryBoardDecisionRepository.Update(decision as ConversionAdvisoryBoardDecision);

		try
		{
await _advisoryBoardDecisionRepository.UnitOfWork.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			var e = ex;
			throw;
		}
		
	}
}
