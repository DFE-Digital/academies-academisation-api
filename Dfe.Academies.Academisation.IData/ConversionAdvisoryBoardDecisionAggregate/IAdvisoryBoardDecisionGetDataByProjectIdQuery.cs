using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionGetDataByProjectIdQuery
{
	Task<IConversionAdvisoryBoardDecision?> Execute(int projectId, bool isTransfer = false);
}
