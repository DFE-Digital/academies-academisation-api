using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class LegacyProjectGetStatusesTests
{
	private readonly ProjectController _projectController;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public LegacyProjectGetStatusesTests()
	{
		_context = new TestProjectContext().CreateContext();

		IProjectStatusesDataQuery dataQuery = new ProjectStatusesDataQuery(_context);
		IProjectGetStatusesQuery query = new ProjectGetStatusesQuery(dataQuery);

		_projectController = new ProjectController(Mock.Of<ILegacyProjectGetQuery>(), Mock.Of<ILegacyProjectListGetQuery>(),
			query, Mock.Of<ILegacyProjectUpdateCommand>(), Mock.Of<ILegacyProjectAddNoteCommand>(), Mock.Of<ILegacyProjectDeleteNoteCommand>(), Mock.Of<ICreateSponsoredProjectCommand>());
	}

	[Fact]
	public async Task ProjectExists___ProjectReturned()
	{
		var projectDetails1 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, "Active").Create();
		
		var projectDetails2 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, "Closed").Create();

		_context.Add(new Project(1,projectDetails1));
		_context.Add(new Project(2,projectDetails2));
		await _context.SaveChangesAsync();

		// act
		var result = await _projectController.GetFilterParameters();

		// assert
		var response = DfeAssert.OkObjectResult(result);

		Assert.Multiple(
			() => Assert.Equal("Active", response.Item2.Statuses![0]),
			() => Assert.Equal("Closed", response.Item2.Statuses![1]),
			() => Assert.Equal(2, response.Item2.Statuses!.Count)
		);
	}
}
