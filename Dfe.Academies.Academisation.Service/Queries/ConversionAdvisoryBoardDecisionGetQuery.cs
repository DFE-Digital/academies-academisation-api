using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Queries;

public class ConversionAdvisoryBoardDecisionGetQuery : IConversionAdvisoryBoardDecisionGetQuery
{
	private readonly IAdvisoryBoardDecisionGetDataByProjectIdQuery _query;

	public ConversionAdvisoryBoardDecisionGetQuery(IAdvisoryBoardDecisionGetDataByProjectIdQuery query)
	{
		_query = query;
	}

	public async Task<ConversionAdvisoryBoardDecisionServiceModel?> Execute(int projectId, bool isTransfer = false)
	{
		var decision = await _query.Execute(projectId, isTransfer);
		return decision?.MapFromDomain();
	}
}
