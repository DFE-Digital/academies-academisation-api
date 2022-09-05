using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IApplicationListByUserQuery
	{
		Task<IList<ApplicationServiceModel>> Execute(string userEmail);
	}
}
