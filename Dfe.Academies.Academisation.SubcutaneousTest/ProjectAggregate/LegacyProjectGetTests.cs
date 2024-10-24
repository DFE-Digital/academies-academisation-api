﻿using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
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
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;
	public ProjectGetTests()
	{ 
		_context = new TestProjectContext(new Mock<IMediator>().Object).CreateContext();
		_mockAdvisoryBoardDecisionRepository = new();
		_projectController = new ProjectController(new ConversionProjectQueryService(new ConversionProjectRepository(_context), new FormAMatProjectRepository(_context), _mockAdvisoryBoardDecisionRepository.Object), Mock.Of<IMediator>());
	}

	[Fact]
	public async Task ProjectExists___ProjectReturned()
	{
		var existingProject = _fixture.Create<Project>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		// act
		ActionResult<ConversionProjectServiceModel> result = await _projectController.Get(existingProject.Id, default);

		// assert
		result.Result.Should().BeOfType<OkObjectResult>();

		var serviceModel = result.Result.As<OkObjectResult>().Value.As<ConversionProjectServiceModel>();

		existingProject.Details.Should().BeEquivalentTo(serviceModel, options => options.ComparingByMembers<ConversionProjectServiceModel>()
		.Excluding(x => x.Notes)
		.Excluding(x => x.Id)
		.Excluding(x => x.CreatedOn)
		.Excluding(x => x.FormAMatProjectId)
		.Excluding(x => x.IsFormAMat)
		.Excluding(x => x.ApplicationSharePointId)
		.Excluding(x => x.SchoolSharePointId)
		.Excluding(x => x.IsReadOnly)
		.Excluding(x => x.ProjectSentToCompleteDate)
		);

		existingProject.Id.Should().Be(serviceModel.Id);
		existingProject.Notes.Should().BeEquivalentTo(serviceModel.Notes);
		existingProject.CreatedOn.Should().Be(serviceModel.CreatedOn);

	}
}
