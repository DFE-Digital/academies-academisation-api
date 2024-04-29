using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionGetDataByDecisionIdQuery
{
	Task<IConversionAdvisoryBoardDecision?> Execute(int decisionId);
}
