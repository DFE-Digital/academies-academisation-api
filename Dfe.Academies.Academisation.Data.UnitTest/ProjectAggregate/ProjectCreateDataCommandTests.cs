using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectCreateDataCommandTests
{
	private readonly Fixture _fixture = new();

	private readonly ProjectCreateDataCommand _subject;
	private readonly AcademisationContext _context;

	public ProjectCreateDataCommandTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ProjectCreateDataCommand(_context);
	}

	[Fact]
	public async Task NoPreconditions___ProjectCreated()
	{
		// arrange
		ProjectDetails projectDetails = _fixture.Create<ProjectDetails>();

		IProject project = new Project(0, projectDetails);

		// act
		var result = await _subject.Execute(project);

		// assert
		Assert.Multiple(
			() => Assert.NotEqual(0, result.Id),
			() => Assert.Equal(projectDetails, result.Details)
		);
	}
}
