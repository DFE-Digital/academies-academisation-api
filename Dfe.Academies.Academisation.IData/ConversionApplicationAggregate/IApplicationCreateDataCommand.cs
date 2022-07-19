using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;

public interface IApplicationCreateDataCommand
{
	Task Execute(IConversionApplication conversionApplication);
}
