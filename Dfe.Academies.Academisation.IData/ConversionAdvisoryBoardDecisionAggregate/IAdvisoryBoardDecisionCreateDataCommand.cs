using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionCreateDataCommand
{
    Task<int> Execute(IConversionAdvisoryBoardDecision decision);
}
