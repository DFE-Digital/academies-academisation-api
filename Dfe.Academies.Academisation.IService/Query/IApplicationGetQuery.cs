using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IApplicationGetQuery
	{
		Task<ApplicationServiceModel?> Execute(int id);
	}
}
