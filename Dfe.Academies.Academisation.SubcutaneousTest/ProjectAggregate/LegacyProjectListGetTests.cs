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
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class LegacyProjectListGetTests
{
	private readonly LegacyProjectController _subject;
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();

	public LegacyProjectListGetTests()
	{
		_context = new TestProjectContext().CreateContext();

		IProjectListGetDataQuery projectGetDataQuery = new ProjectListGetDataQuery(_context);
		ILegacyProjectListGetQuery legacyProjectGetQuery = new LegacyProjectListGetQuery(projectGetDataQuery);

		_subject = new LegacyProjectController(Mock.Of<ILegacyProjectGetQuery>(), legacyProjectGetQuery,
			Mock.Of<IProjectGetStatusesQuery>(), Mock.Of<ILegacyProjectUpdateCommand>());
	}

	[Fact]
	public async Task ProjectsExists___ProjectListReturned()
	{
		var project1 = _fixture.Create<ProjectState>();
		var project2 = _fixture.Create<ProjectState>();
		var project3 = _fixture.Create<ProjectState>();

		await _context.Projects.AddAsync(project1);
		await _context.Projects.AddAsync(project2);
		await _context.Projects.AddAsync(project3);
		await _context.SaveChangesAsync();

		GetAcademyConversionSearchModel searchModel = new GetAcademyConversionSearchModel(1, 3, null, null, null, null);
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
		var project1 = _fixture.Create<ProjectState>();
		var project2 = _fixture.Create<ProjectState>();
		var project3 = _fixture.Create<ProjectState>();

		await _context.Projects.AddAsync(project1);
		await _context.Projects.AddAsync(project2);
		await _context.Projects.AddAsync(project3);
		await _context.SaveChangesAsync();

		int[] regions =  { project1.Urn, project2.Urn };
		GetAcademyConversionSearchModel searchModel = new GetAcademyConversionSearchModel(1, 3, null, null, regions, null);
		// act
		var result = await _subject.GetProjects(searchModel);

		// assert
		var (_, getProjects) = DfeAssert.OkObjectResult(result);

		Assert.Equal(2, getProjects.Data.Count());
	}
}
