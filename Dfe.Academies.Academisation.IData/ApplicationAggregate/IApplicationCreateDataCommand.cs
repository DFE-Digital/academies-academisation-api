using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate;

public interface IApplicationCreateDataCommand
{
	Task Execute(Application application);
}
