using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IData.ApplicationAggregate
{
	public interface IApplicationUpdateDataCommand
	{
		Task Execute(IApplication application);
	}
}
