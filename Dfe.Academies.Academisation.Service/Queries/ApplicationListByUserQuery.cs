using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationListByUserQuery : IApplicationListByUserQuery
	{
		private readonly IApplicationsListByUserDataQuery _applicationListByUserDataQuery;

		public ApplicationListByUserQuery(IApplicationsListByUserDataQuery applicationListByUserDataQuery)
		{
			_applicationListByUserDataQuery = applicationListByUserDataQuery;
		}

		public async Task<IList<ApplicationServiceModel>> Execute(string userEmail)
		{
			var applications = await _applicationListByUserDataQuery.Execute(userEmail);
			return applications.Select(a => a.MapFromDomain()).ToList();
		}
	}
}
