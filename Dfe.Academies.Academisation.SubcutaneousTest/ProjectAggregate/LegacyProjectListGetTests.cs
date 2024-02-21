using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class LegacyProjectListGetTests
{
	private readonly ProjectController _subject;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public LegacyProjectListGetTests()
	{
		_context = new TestProjectContext().CreateContext();

		_subject = new ProjectController(Mock.Of<ICreateNewProjectCommand>(), new ConversionProjectQueryService(new ConversionProjectRepository(_context, null), new FormAMatProjectRepository(_context)), Mock.Of<IMediator>());
	}

	[Fact]
	public async Task ProjectsExists___ProjectListReturned()
	{
		var project1 = _fixture.Create<Project>();
		var project2 = _fixture.Create<Project>();
		var project3 = _fixture.Create<Project>();

		await _context.Projects.AddAsync(project1);
		await _context.Projects.AddAsync(project2);
		await _context.Projects.AddAsync(project3);
		await _context.SaveChangesAsync();

		GetAcademyConversionSearchModel searchModel = new GetAcademyConversionSearchModel(1, 3, null, null, null, null, null);
		// act
		var result = await _subject.GetProjects(searchModel);

		// assert
		var (_, getProjects) = DfeAssert.OkObjectResult(result);

		Assert.Equal(3, getProjects.Data.Count());
	}
	[Fact]
	public async Task ProjectsExists_ProjectListReturned_WithRegionFilter()
	{
		// arrange 
		var project1 = _fixture.Create<Project>();
		var project2 = _fixture.Create<Project>();
		var project3 = _fixture.Create<Project>();

		await _context.Projects.AddAsync(project1);
		await _context.Projects.AddAsync(project2);
		await _context.Projects.AddAsync(project3);
		await _context.SaveChangesAsync();

		string[] regions = { project1.Details.Region!.ToLower(), project2.Details.Region!.ToLower() };
		GetAcademyConversionSearchModel searchModel = new GetAcademyConversionSearchModel(1, 3, null, null, regions, null, null);
		// act
		var result = await _subject.GetProjects(searchModel);

		// assert
		var (_, getProjects) = DfeAssert.OkObjectResult(result);

		Assert.Equal(2, getProjects.Data.Count());
	}
	[Fact]
	public async Task ProjectsExists_ProjectListReturned_WithApplicationIdFilter()
	{
		// arrange 
		var project1 = _fixture.Create<Project>();
		var project2 = _fixture.Create<Project>();
		var project3 = _fixture.Create<Project>();

		await _context.Projects.AddAsync(project1);
		await _context.Projects.AddAsync(project2);
		await _context.Projects.AddAsync(project3);
		await _context.SaveChangesAsync();

		string[] applicationReferences = { project1.Details.ApplicationReferenceNumber!, project2.Details.ApplicationReferenceNumber! };
		GetAcademyConversionSearchModel searchModel = new GetAcademyConversionSearchModel(1, 3, null, null, null, null, applicationReferences);
		// act
		var result = await _subject.GetProjects(searchModel);

		// assert
		var (_, getProjects) = DfeAssert.OkObjectResult(result);

		Assert.Equal(2, getProjects.Data.Count());
	}
}
