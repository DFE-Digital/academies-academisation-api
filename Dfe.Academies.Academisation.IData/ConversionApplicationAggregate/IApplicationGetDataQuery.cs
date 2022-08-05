using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate
{
	public interface IApplicationGetDataQuery
	{
		Task<IApplication?> Execute(int id);
	}
}
