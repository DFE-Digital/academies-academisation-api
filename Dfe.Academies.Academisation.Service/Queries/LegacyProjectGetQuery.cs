using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries;

public class LegacyProjectGetQuery : ILegacyProjectGetQuery
{
	private readonly IProjectGetDataQuery _dataQuery;

	public LegacyProjectGetQuery(IProjectGetDataQuery dataQuery)
	{
		_dataQuery = dataQuery;
	}

	public async Task<LegacyProjectServiceModel?> Execute(int id)
	{
		IProject? project = await _dataQuery.Execute(id);

		return project?.MapToServiceModel();
	}
}
