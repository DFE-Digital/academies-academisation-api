using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public class ProjectGetDataQuery : IProjectGetDataQuery
{
	private readonly Fixture _fixture = new();

	public async Task<IProject?> Execute(int id)
	{
		ProjectDetails projectDetails = _fixture.Create<ProjectDetails>();

		await Task.CompletedTask;

		return new Project(id, projectDetails);
	}
}
