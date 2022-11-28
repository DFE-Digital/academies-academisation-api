using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class LegacyProjectGetStatusesTests
{
	private readonly LegacyProjectController _legacyProjectController;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public LegacyProjectGetStatusesTests()
	{
		_context = new TestProjectContext().CreateContext();		

		IProjectStatusesDataQuery dataQuery = new ProjectStatusesDataQuery(_context);
		IProjectGetStatusesQuery query = new ProjectGetStatusesQuery(dataQuery);

		_legacyProjectController = new LegacyProjectController(Mock.Of<ILegacyProjectGetQuery>(), Mock.Of<ILegacyProjectListGetQuery>(),
			query, Mock.Of<ILegacyProjectUpdateCommand>());
	}

	[Fact]
	public async Task ProjectExists___ProjectReturned()
	{
		var existingProject = _fixture.Create<ProjectState>();
		_context.Add(new ProjectState() { ProjectStatus = "Active" });
		_context.Add(new ProjectState() { ProjectStatus = "Closed" });
		_context.SaveChanges();

		// act
		var result = await _legacyProjectController.GetFilterParameters();

		// assert
		var response = DfeAssert.OkObjectResult(result);

		Assert.Multiple(
			() => Assert.Equal("Active", response.Item2.Statuses[0]),
			() => Assert.Equal("Closed", response.Item2.Statuses[1]),
			() => Assert.Equal(2, response.Item2.Statuses.Count)
		);
	}	
}
