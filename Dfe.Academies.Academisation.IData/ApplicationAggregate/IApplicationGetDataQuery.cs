using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate
{
	public interface IApplicationGetDataQuery
	{
		Task<IApplication?> Execute(int id);
	}
}
