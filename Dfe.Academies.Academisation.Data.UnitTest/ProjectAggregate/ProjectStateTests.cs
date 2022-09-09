using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectStateTests
{
	private readonly Fixture _fixture = new();

	[Fact]
	public void MapFromDomain_MapToDomain___NoPreconditions___DetailsEqual()
	{
		// arrange
		ProjectDetails projectDetails = _fixture.Create<ProjectDetails>();
		Project project = new(3, projectDetails);

		// act
		var mapped = ProjectState.MapFromDomain(project);
		var doubleMapped = mapped.MapToDomain();

		Assert.Equal(projectDetails, doubleMapped.Details);
	}
}
