using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate;

public interface IProjectUpdateDataCommand
{
	Task Execute(IProject project);
}
