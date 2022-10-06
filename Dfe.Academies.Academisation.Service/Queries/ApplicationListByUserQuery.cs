using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationListByUserQuery : IApplicationListByUserQuery
	{
		private readonly IApplicationsListByUserDataQuery _applicationListByUserDataQuery;
		private readonly IMapper _mapper;

		public ApplicationListByUserQuery(IApplicationsListByUserDataQuery applicationListByUserDataQuery, IMapper mapper)
		{
			_applicationListByUserDataQuery = applicationListByUserDataQuery;
			_mapper = mapper;
		}

		public async Task<IList<ApplicationServiceModel>> Execute(string userEmail)
		{
			var applications = await _applicationListByUserDataQuery.Execute(userEmail);
			return applications.Select(a => a.MapFromDomain(_mapper)).ToList();
		}
	}
}
