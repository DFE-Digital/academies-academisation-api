using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;

public interface IApplicationCreateDataCommand
{
	Task Execute(IApplication conversionApplication);
}
