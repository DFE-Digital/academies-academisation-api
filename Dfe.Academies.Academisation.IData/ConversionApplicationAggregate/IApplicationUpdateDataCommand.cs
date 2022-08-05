using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate
{
	public interface IApplicationUpdateDataCommand
	{
		Task Execute(IApplication conversionApplication);
	}
}
