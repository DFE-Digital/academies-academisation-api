using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate;

public interface IProjectCreateDataCommand
{
	Task<IProject> Execute(IProject project);
}
