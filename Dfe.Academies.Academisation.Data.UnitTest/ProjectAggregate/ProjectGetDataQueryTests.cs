using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectGetDataQueryTests
{
	private readonly Fixture _fixture = new();

	private readonly ProjectGetDataQuery _subject;
	private readonly AcademisationContext _context;

	public ProjectGetDataQueryTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ProjectGetDataQuery(_context);
	}

	[Fact]
	public async Task ProjectExists___GetProject()
	{
		// arrange
		var projectDetails = _fixture.Create<ProjectDetails>();
		var newProject = new Project(0, projectDetails);
		var mappedProject = ProjectState.MapFromDomain(newProject);

		_context.Projects.Add(mappedProject);
		await _context.SaveChangesAsync();

		// act
		var result = await _subject.Execute(mappedProject.Id);

		// assert
		Assert.NotNull(result);
		Assert.Equal(mappedProject.Id, result.Id);
		Assert.Equal(projectDetails, result.Details);
	}
}
