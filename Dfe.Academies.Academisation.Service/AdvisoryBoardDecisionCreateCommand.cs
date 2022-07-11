
using Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Service;

public class AdvisoryBoardDecisionCreateCommand
{
	private readonly IAdvisoryBoardDecisionFactory _factory;

	public AdvisoryBoardDecisionCreateCommand(IAdvisoryBoardDecisionFactory factory)
	{
		_factory = factory;
	}

	public async Task<IAdvisoryBoardDecision> Create(IAdvisoryBoardDecisionDetails details)
	{
		var advisoryBoardDecision = await _factory.Create(details);

		//ToDo: Save to Database

		return advisoryBoardDecision;
	}
}
