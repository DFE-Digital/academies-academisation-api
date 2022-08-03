using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service.Queries;

public class ConversionAdvisoryBoardDecisionGetQuery : IConversionAdvisoryBoardDecisionGetQuery
{
	private readonly IAdvisoryBoardDecisionGetDataQuery _query;

	public ConversionAdvisoryBoardDecisionGetQuery(IAdvisoryBoardDecisionGetDataQuery query)
	{
		_query = query;
	}

	public async Task<ConversionAdvisoryBoardDecisionServiceModel?> Execute(int projectId)
	{
		var decision = await _query.Execute(projectId);
		return decision?.MapFromDomain();
	}
}
