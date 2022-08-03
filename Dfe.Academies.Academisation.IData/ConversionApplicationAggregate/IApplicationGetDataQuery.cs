using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate
{
	public interface IApplicationGetDataQuery
	{
		Task<IConversionApplication?> Execute(int id);
	}
}
