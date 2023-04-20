using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IApplicationQueryService
	{
		Task<ApplicationServiceModel?> GetById(int id);
		Task<List<ApplicationServiceModel>> GetByUserEmail(string email);
		Task<ApplicationServiceModel?> GetByApplicationReference(string applicationReference);
		Task<List<ApplicationSchoolSharepointServiceModel>> GetAllApplications();
	}
}
