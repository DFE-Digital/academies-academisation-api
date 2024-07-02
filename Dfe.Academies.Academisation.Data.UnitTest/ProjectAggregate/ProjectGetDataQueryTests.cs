using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectGetDataQueryTests
{
	private readonly Fixture _fixture = new();

	private readonly ConversionProjectRepository _subject;
	private readonly AcademisationContext _context;
	private readonly IMediator _mediator;
	public ProjectGetDataQueryTests()
	{
		_context = new TestProjectContext(_mediator).CreateContext();
		_subject = new ConversionProjectRepository(_context, null);
	}

	[Fact]
	public async Task ProjectExists___GetProject()
	{
		// arrange
		var projectDetails = _fixture.Create<ProjectDetails>();
		var newProject = new Project(0, projectDetails);

		_context.Projects.Add(newProject);
		await _context.SaveChangesAsync();

		// act
		var result = await _subject.GetConversionProject(newProject.Id, default);

		// assert
		Assert.NotNull(result);
		Assert.Equal(newProject.Id, result.Id);
		Assert.Equal(projectDetails, result.Details);
	}
}
