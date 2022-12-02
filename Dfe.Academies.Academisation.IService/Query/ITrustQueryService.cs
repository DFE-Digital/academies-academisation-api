using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITrustQueryService
	{
		Task<TrustKeyPersonServiceModel> GetTrustKeyPerson(int applicationId, int keyPersonId);
		Task<List<TrustKeyPersonServiceModel>> GetAllTrustKeyPeople(int applicationId);
	}
}
