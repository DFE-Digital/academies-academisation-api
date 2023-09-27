using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectGetTests
{
	private readonly ProjectController _projectController;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public ProjectGetTests()
	{
		_context = new TestProjectContext().CreateContext();

		IProjectGetDataQuery projectGetDataQuery = new ProjectGetDataQuery(_context);
		ILegacyProjectGetQuery legacyProjectGetQuery = new LegacyProjectGetQuery(projectGetDataQuery);

		_projectController = new ProjectController(legacyProjectGetQuery, Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), Mock.Of<ILegacyProjectUpdateCommand>(), Mock.Of<ILegacyProjectAddNoteCommand>(),
			Mock.Of<ILegacyProjectDeleteNoteCommand>(), Mock.Of<ICreateSponsoredProjectCommand>());
	}

	[Fact]
	public async Task ProjectExists___ProjectReturned()
	{
		var existingProject = _fixture.Create<Project>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		// act
		ActionResult<LegacyProjectServiceModel> result = await _projectController.Get(existingProject.Id);

		// assert
		result.Result.Should().BeOfType<OkObjectResult>();

		result.Result.As<OkObjectResult>().Value.Should()
			.BeEquivalentTo(existingProject, options =>
				options.Excluding(x => x.Details.Notes)
					.Excluding(x => x.Details.AssignedDate)
					.Excluding(x => x.Details.AssignedUser.Id)
					.Excluding(x => x.Details.AssignedUser.EmailAddress)
					.Excluding(x => x.Details.AssignedUser.FullName)); ;
	}	
}
