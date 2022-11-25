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
		Task<TrustKeyPerson> GetTrustKeyPerson(int applicationId, int keyPersonId);
		Task<List<TrustKeyPerson>> GetAllTrustKeyPeople(int applicationId);
	}
}
