using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate;

public interface IApplicationCreateDataCommand
{
	Task Execute(IApplication application);
}
