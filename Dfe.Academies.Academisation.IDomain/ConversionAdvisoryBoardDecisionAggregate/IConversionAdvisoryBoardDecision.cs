using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecision
{
	int Id { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }
	AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }
	void SetId(int id);
	CommandResult Update(AdvisoryBoardDecisionDetails details);
}
