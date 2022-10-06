using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationGetQuery : IApplicationGetQuery
	{
		private readonly IApplicationGetDataQuery _applicationGetDataQuery;
		private readonly IMapper _mapper;

		public ApplicationGetQuery(IApplicationGetDataQuery applicationGetDataQuery, IMapper mapper)
		{
			_applicationGetDataQuery = applicationGetDataQuery;
			_mapper = mapper;
		}

		public async Task<ApplicationServiceModel?> Execute(int id)
		{
			var application = await _applicationGetDataQuery.Execute(id);
			return application?.MapFromDomain(_mapper);
		}
	}
}
