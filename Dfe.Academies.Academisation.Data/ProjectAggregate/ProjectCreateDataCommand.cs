using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public class ProjectCreateDataCommand : IProjectCreateDataCommand
{	
	public async Task<IProject> Execute(IProject project)
	{
		await Task.CompletedTask;

		return new Project(1, project.Details);
	}
}
