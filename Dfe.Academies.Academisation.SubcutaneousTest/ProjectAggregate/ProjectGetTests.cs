using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectGetTests
{
	private readonly Fixture _fixture = new();

	private readonly IProjectGetDataQuery _projectGetDataQuery;
	private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
	private readonly LegacyProjectController _legacyProjectController;

	public ProjectGetTests()
	{
		_projectGetDataQuery = new ProjectGetDataQuery();
		_legacyProjectGetQuery = new LegacyProjectGetQuery(_projectGetDataQuery);

		_legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery);
	}

	[Fact]
	public async Task NoPreconditions___ProjectReturned()
	{
		int id = _fixture.Create<int>();

		// act
		var result = await _legacyProjectController.Get(id);

		// assert
		Assert.IsType<OkObjectResult>(result.Result);
	}
}
