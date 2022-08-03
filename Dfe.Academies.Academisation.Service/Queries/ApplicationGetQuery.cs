using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationGetQuery : IApplicationGetQuery
	{
		private readonly IApplicationGetDataQuery _applicationGetDataQuery;

		public ApplicationGetQuery(IApplicationGetDataQuery applicationGetDataQuery)
		{
			_applicationGetDataQuery = applicationGetDataQuery;
		}

		public async Task<ApplicationServiceModel?> Execute(int id)
		{
			var application = await _applicationGetDataQuery.Execute(id);
			return application?.MapFromDomain();
		}
	}
}
