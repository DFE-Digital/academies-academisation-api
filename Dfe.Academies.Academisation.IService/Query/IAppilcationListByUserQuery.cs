using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	// NOT IMPLEMENTED
	public interface IApplicationListByUserQuery
	{
		Task<IList<ApplicationServiceModel>> Execute(string userEmail);
	}
}
