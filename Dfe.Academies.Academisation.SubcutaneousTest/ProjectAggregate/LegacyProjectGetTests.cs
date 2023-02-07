using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.Project;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectGetTests
{
	private readonly LegacyProjectController _legacyProjectController;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public ProjectGetTests()
	{
		_context = new TestProjectContext().CreateContext();

		IProjectGetDataQuery projectGetDataQuery = new ProjectGetDataQuery(_context);
		ILegacyProjectGetQuery legacyProjectGetQuery = new LegacyProjectGetQuery(projectGetDataQuery);

		_legacyProjectController = new LegacyProjectController(legacyProjectGetQuery, Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), Mock.Of<ILegacyProjectUpdateCommand>(), Mock.Of<ILegacyProjectAddNoteCommand>(),
			Mock.Of<ILegacyProjectDeleteNoteCommand>(), Mock.Of<ICreateInvoluntaryProjectCommand>());
	}

	[Fact]
	public async Task ProjectExists___ProjectReturned()
	{
		var existingProject = _fixture.Create<ProjectState>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		// act
		var result = await _legacyProjectController.Get(existingProject.Id);

		// assert
		Assert.IsType<OkObjectResult>(result.Result);
	}	
}
