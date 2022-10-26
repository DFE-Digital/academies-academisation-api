using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate
{
	public interface IApplicationsListByUserDataQuery
	{
		Task<IList<Application>> Execute(string userEmail);
	}
}
