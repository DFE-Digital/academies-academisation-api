using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionUpdateDataCommand
{
	Task Execute(IConversionAdvisoryBoardDecision decision);
}
