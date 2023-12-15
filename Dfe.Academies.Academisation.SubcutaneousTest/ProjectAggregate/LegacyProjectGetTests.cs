using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
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

		_projectController = new ProjectController(Mock.Of<ICreateNewProjectCommand>(), new ConversionProjectQueryService(new ConversionProjectRepository(_context, null)), Mock.Of<IMediator>());
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

		var serviceModel = result.Result.As<OkObjectResult>().Value.As<LegacyProjectServiceModel>();

		existingProject.Details.Should().BeEquivalentTo(serviceModel, options => options.ComparingByMembers<LegacyProjectServiceModel>()
		.Excluding(x => x.Notes)
		.Excluding(x => x.Id)
		.Excluding(x => x.CreatedOn)
		);

		existingProject.Id.Should().Be(serviceModel.Id);
		existingProject.Notes.Should().BeEquivalentTo(serviceModel.Notes);
		existingProject.CreatedOn.Should().Be(serviceModel.CreatedOn);

	}
}
