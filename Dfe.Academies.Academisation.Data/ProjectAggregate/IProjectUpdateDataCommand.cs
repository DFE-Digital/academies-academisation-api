using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public interface IProjectUpdateDataCommand
{
	Task Execute(IProject project);
}
