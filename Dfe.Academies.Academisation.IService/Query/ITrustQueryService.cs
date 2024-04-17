using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITrustQueryService
	{
		Task<TrustKeyPersonServiceModel> GetTrustKeyPerson(int applicationId, int keyPersonId);
		Task<List<TrustKeyPersonServiceModel>> GetAllTrustKeyPeople(int applicationId);
	}
}
