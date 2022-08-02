using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionGetDataQuery
{
	Task<IConversionAdvisoryBoardDecision?> Execute(int id);
}
