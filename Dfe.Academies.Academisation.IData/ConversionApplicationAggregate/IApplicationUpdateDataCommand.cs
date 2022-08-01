using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate
{
	public interface IApplicationUpdateDataCommand
	{
		Task Execute(IConversionApplication conversionApplication);
	}
}
