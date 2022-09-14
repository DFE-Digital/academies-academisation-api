using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectGetTests
{
	private readonly Fixture _fixture = new();

	private readonly IProjectGetDataQuery _projectGetDataQuery;
	private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
	private readonly LegacyProjectController _legacyProjectController;
	private readonly AcademisationContext _context;


	public ProjectGetTests()
	{
		_context = new TestProjectContext().CreateContext();

		_projectGetDataQuery = new ProjectGetDataQuery(_context);
		_legacyProjectGetQuery = new LegacyProjectGetQuery(_projectGetDataQuery);

		_legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, Mock.Of<ILegacyProjectUpdateCommand>());
	}

	[Fact]
	public async Task NoPreconditions___ProjectReturned()
	{
		int id = 1;

		// act
		var result = await _legacyProjectController.Get(id);

		// assert
		Assert.IsType<OkObjectResult>(result.Result);
	}	
}
