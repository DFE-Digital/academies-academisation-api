using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate
{
	public interface IApplicationsListByUserDataQuery
	{
		Task<IList<IApplication>> Execute(string userEmail);
	}
}
