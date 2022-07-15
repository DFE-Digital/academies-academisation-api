using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IData;

public interface IApplicationCreateDataCommand
{
	Task Execute(IConversionApplication conversionApplication);
}
