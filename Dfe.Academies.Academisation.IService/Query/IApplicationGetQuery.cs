using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService
{
	public interface IApplicationGetQuery
	{
		Task<ApplicationServiceModel?> Execute(int id);
	}
}
