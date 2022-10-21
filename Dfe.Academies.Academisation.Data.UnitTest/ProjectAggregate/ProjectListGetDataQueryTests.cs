using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectsListGetDataQueryTests
{
	private readonly Fixture _fixture = new();

	private readonly ProjectListGetDataQuery _subject;
	private readonly AcademisationContext _context;

	public ProjectsListGetDataQueryTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ProjectListGetDataQuery(_context);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByStatus__ReturnsProjects()
	{
		// arrange
		(ProjectDetails projectDetails1, ProjectState projectState1) = CreateTestProject(DateTime.Now);
		(ProjectDetails projectDetails2, ProjectState projectState2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, ProjectState projectState3) = CreateTestProject();

		_context.Projects.Add(projectState1);
		_context.Projects.Add(projectState2);
		_context.Projects.Add(projectState3);

		await _context.SaveChangesAsync();

		// act
		var searchStatus =
			new List<string> { projectState1.ProjectStatus!.ToLower(), projectState2.ProjectStatus!.ToLower() };
		var projects = (await _subject.SearchProjects(searchStatus, null, 1, 10, null)).Item1.ToList();

		// assert
		var firstProject = projects.FirstOrDefault();
		var secondProject = projects.LastOrDefault();

		Assert.Multiple(
			() => Assert.NotNull(firstProject),
			() => Assert.Equal(firstProject!.Details, projectDetails1),
			() => Assert.Equal(projectState1.Id, firstProject!.Id),
			() => Assert.NotNull(secondProject),
			() => Assert.Equal(secondProject!.Details, projectDetails2),
			() => Assert.Equal(projectState2.Id, secondProject!.Id)
		);
	}


	[Fact]
	public async Task ProjectsExists_SearchProjectsByTitle__ReturnsProjects()
	{
		// arrange
		string searchTerm = "Bristol";
		for (int i = 0; i < 6; i++)
		{
			(ProjectDetails projectDetails, ProjectState projectState) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i % 2 == 0) projectState.SchoolName = $"{searchTerm} {i}";
			_context.Projects.Add(projectState);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, searchTerm, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal($"{searchTerm} 0", projects[0].Details.SchoolName),
			() => Assert.Equal($"{searchTerm} 2", projects[1].Details.SchoolName),
			() => Assert.Equal($"{searchTerm} 4", projects[2].Details.SchoolName)
		);
	}	

	[Fact]
	public async Task ProjectsExists_SearchProjectsByUrn__ReturnsProjects()
	{
		// arrange
		(_, ProjectState projectState1) = CreateTestProject(DateTime.Now);
		(ProjectDetails projectDetails2, ProjectState projectState2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, ProjectState projectState3) = CreateTestProject();

		_context.Projects.Add(projectState1);
		_context.Projects.Add(projectState2);
		_context.Projects.Add(projectState3);

		await _context.SaveChangesAsync();

		// act
		int searchUrn = projectState2.Urn;
		var projects = (await _subject.SearchProjects(null, null, 1, 10, searchUrn)).Item1.ToList();

		// assert
		var firstProject = projects.FirstOrDefault();

		Assert.Multiple(
			() => Assert.NotNull(firstProject),
			() => Assert.Equal(firstProject!.Details, projectDetails2),
			() => Assert.Equal(projectState2.Id, firstProject!.Id)
		);
	}

	[Fact]
	public async Task ProjectsExists__GetPagedProjects__ReturnsTotalCount()
	{
		// arrange
		for (int i = 0; i < 6; i++)
		{
			(ProjectDetails projectDetails, ProjectState projectState) = CreateTestProject(DateTime.Now.AddDays(-i));
			_context.Projects.Add(projectState);
		}

		await _context.SaveChangesAsync();

		// act
		var (firstPageProjects, firstPageCount) = await _subject.SearchProjects(new List<string>(), null, 1, 2, null);
		var (secondPageProjects, secondPageCount) = await _subject.SearchProjects(new List<string>(), null, 2, 2, null);

		// assert
		Assert.Multiple(
			() => Assert.Equal(6, firstPageCount),
			() => Assert.Equal(2, firstPageProjects.Count()),
			() => Assert.Equal(6, firstPageCount),
			() => Assert.Equal(2, secondPageProjects.Count()),
			() => Assert.Equal(6, secondPageCount)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchDoesntMatch__ReturnsEmpty()
	{
		// arrange
		(_, ProjectState projectState1) = CreateTestProject(DateTime.Now);
		(_, ProjectState projectState2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, ProjectState projectState3) = CreateTestProject();

		_context.Projects.Add(projectState1);
		_context.Projects.Add(projectState2);
		_context.Projects.Add(projectState3);

		await _context.SaveChangesAsync();

		// act
		int urnThatDoesNotExist = 12312;
		var projects = (await _subject.SearchProjects(null, null, 1, 10, urnThatDoesNotExist)).Item1.ToList();

		// assert
		Assert.Empty(projects);
	}

	[Fact]
	public async Task ProjectsExists__GetPagedProjects__ReturnsProjects()
	{
		// arrange
		List<ProjectDetails> createdProjects = new List<ProjectDetails>();
		for (int i = 0; i < 6; i++)
		{
			(ProjectDetails projectDetails, ProjectState projectState) = CreateTestProject(DateTime.Now.AddDays(-i));
			createdProjects.Add(projectDetails);
			_context.Projects.Add(projectState);
		}

		await _context.SaveChangesAsync();

		// act
		var firstPage = (await _subject.SearchProjects(new List<string>(), null, 1, 2, null)).Item1.ToList();
		var secondPage = (await _subject.SearchProjects(new List<string>(), null, 2, 2, null)).Item1.ToList();
		var thirdPage = (await _subject.SearchProjects(new List<string>(), null, 3, 2, null)).Item1.ToList();

		// assert
		Assert.Multiple(
			() => Assert.Equal(createdProjects[0], firstPage.First().Details),
			() => Assert.Equal(createdProjects[1], firstPage.Last().Details),
			() => Assert.Equal(createdProjects[2], secondPage.First().Details),
			() => Assert.Equal(createdProjects[3], secondPage.Last().Details),
			() => Assert.Equal(createdProjects[4], thirdPage.First().Details),
			() => Assert.Equal(createdProjects[5], thirdPage.Last().Details)
		);
	}

	private (ProjectDetails, ProjectState) CreateTestProject(DateTime? applicationDate = null)
	{
		applicationDate ??= DateTime.Now;

		var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.ApplicationReceivedDate, applicationDate).Create();
		var newProject = new Project(0, projectDetails);
		var mappedProject = ProjectState.MapFromDomain(newProject);

		return (projectDetails, mappedProject);
	}
}
