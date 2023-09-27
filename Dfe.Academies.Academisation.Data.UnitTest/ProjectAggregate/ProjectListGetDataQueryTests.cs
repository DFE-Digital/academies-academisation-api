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
		(ProjectDetails projectDetails1, Project project1) = CreateTestProject(DateTime.Now);
		(ProjectDetails projectDetails2, Project project2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, Project project3) = CreateTestProject();

		_context.Projects.Add(project1);
		_context.Projects.Add(project2);
		_context.Projects.Add(project3);

		await _context.SaveChangesAsync();

		// act
		var searchStatus =
			new string[] { project1.Details.ProjectStatus!.ToLower(), project2.Details.ProjectStatus!.ToLower() };
		var projects = (await _subject.SearchProjects(searchStatus, null, null, 1, 10, null)).Item1.ToList();

		// assert
		var firstProject = projects.FirstOrDefault();
		var secondProject = projects.LastOrDefault();

		Assert.Multiple(
			() => Assert.NotNull(firstProject),
			() => Assert.Equal(firstProject!.Details, projectDetails1),
			() => Assert.Equal(project1.Id, firstProject!.Id),
			() => Assert.NotNull(secondProject),
			() => Assert.Equal(secondProject!.Details, projectDetails2),
			() => Assert.Equal(project2.Id, secondProject!.Id)
		);
	}


	[Fact]
	public async Task ProjectsExists_SearchProjectsByTitle__ReturnsProjects()
	{
		// arrange
		string searchTerm = "Bristol";
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i % 2 == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.SchoolName,$"{searchTerm} {i}" ).Create();


				project = AddProjectDetailToProject(projectDetails);
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, searchTerm, null, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal($"{searchTerm} 0", projects[0].Details.SchoolName),
			() => Assert.Equal($"{searchTerm} 2", projects[1].Details.SchoolName),
			() => Assert.Equal($"{searchTerm} 4", projects[2].Details.SchoolName)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByDeliveryOfficer__ReturnsProjects()
	{
		// arrange
		string deliveryOfficer = "Dave";
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i % 2 == 0) 
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, deliveryOfficer).Create();

				project = AddProjectDetailToProject(projectDetails);
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, null, new[] { deliveryOfficer }, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal(deliveryOfficer, projects[0].Details.AssignedUser!.FullName),
			() => Assert.Equal(deliveryOfficer, projects[1].Details.AssignedUser!.FullName),
			() => Assert.Equal(deliveryOfficer, projects[2].Details.AssignedUser!.FullName)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByMultipleDeliveryOfficers__ReturnsProjects()
	{
		// arrange
		string[] deliveryOfficers = new[] { "Dave", "Bob" };
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i < 2) 
			{
					var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, deliveryOfficers[i]).Create();

			project = AddProjectDetailToProject(projectDetails);	
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, null, deliveryOfficers, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(2, projects.Count),
			() => Assert.Equal(deliveryOfficers[0], projects[0].Details.AssignedUser!.FullName),
			() => Assert.Equal(deliveryOfficers[1], projects[1].Details.AssignedUser!.FullName)			
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByMultipleRegions__ReturnsProjects()
	{
		// arrange
		string[] regions = { "east", "west"};
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i < 2)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.Region, regions[i]).Create();

			project = AddProjectDetailToProject(projectDetails);	 
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, null, null, 1, 10, null, regions)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(2, projects.Count),
			() => Assert.Equal(regions[0], projects[0].Details.Region),
			() => Assert.Equal(regions[1], projects[1].Details.Region)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByAllCriteria__ReturnsProject()
	{
		// arrange
		var ( deliveryOfficer, status, title, urn ) = ( "Dave", "active", "school", 1234 );
		for (int i = 0; i < 3; i++)
		{
			(_, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			if (i == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, deliveryOfficer)
			.With(p => p.ProjectStatus, status)
			.With(p => p.SchoolName, title)
			.With(p => p.Urn, urn)
			.Create();

			project = AddProjectDetailToProject(projectDetails);

			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(new[] { status }, title, new[] { deliveryOfficer }, 1, 10, urn))
			.Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Single(projects),
			() => Assert.Equal(projects[0].Details.AssignedUser.FullName, deliveryOfficer),
			() => Assert.Equal(projects[0].Details.ProjectStatus, status),
			() => Assert.Equal(projects[0].Details.SchoolName, title),
			() => Assert.Equal(projects[0].Details.Urn, urn)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchUnassignedProjects__ReturnsProjects()
	{
		// arrange
		string[] deliveryOfficers = new[] { "Dave", "Not assigned" };
		
		(_, Project project1) = CreateTestProject(DateTime.Now);
			var projectDetails1 = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, "Dave").Create();
			project1 = AddProjectDetailToProject(projectDetails1);
		
		//project1.Details.AssignedUser.FullName = "Dave";
		(_, Project project2) = CreateTestProject(DateTime.Now.AddDays(-1));
		     var projectDetails2 = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, "").Create();
			project2 = AddProjectDetailToProject(projectDetails2);

		(_, Project project3) = CreateTestProject(DateTime.Now.AddDays(-2));
	        var projectDetails3 = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser.FullName, "").Create();
			project3 = AddProjectDetailToProject(projectDetails3);

		(_, Project project4) = CreateTestProject(DateTime.Now.AddDays(-3));
		
		await _context.Projects.AddRangeAsync(project1, project2, project3, project4);	
		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, null, deliveryOfficers, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal(deliveryOfficers[0], projects[0].Details.AssignedUser!.FullName),
			() => Assert.Equal(string.Empty, projects[1].Details.AssignedUser?.FullName),
			() => Assert.Equal(string.Empty, projects[2].Details.AssignedUser?.FullName)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByUrn__ReturnsProjects()
	{
		// arrange
		(_, Project project1) = CreateTestProject(DateTime.Now);
		(ProjectDetails projectDetails2, Project project2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, Project project3) = CreateTestProject();

		_context.Projects.Add(project1);
		_context.Projects.Add(project2);
		_context.Projects.Add(project3);

		await _context.SaveChangesAsync();

		// act
		int searchUrn = project2.Details.Urn;
		var projects = (await _subject.SearchProjects(null, null, null, 1, 10, searchUrn)).Item1.ToList();

		// assert
		var firstProject = projects.FirstOrDefault();

		Assert.Multiple(
			() => Assert.NotNull(firstProject),
			() => Assert.Equal(firstProject!.Details, projectDetails2),
			() => Assert.Equal(project2.Id, firstProject!.Id)
		);
	}

	[Fact]
	public async Task ProjectsExists__GetPagedProjects__ReturnsTotalCount()
	{
		// arrange
		for (int i = 0; i < 6; i++)
		{
			(ProjectDetails projectDetails, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act
		var (firstPageProjects, firstPageCount) = await _subject.SearchProjects(new string[] { }, null, null, 1, 2, null);
		var (secondPageProjects, secondPageCount) = await _subject.SearchProjects(new string[] { }, null, null, 2, 2, null);

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
		(_, Project project1) = CreateTestProject(DateTime.Now);
		(_, Project project2) = CreateTestProject(DateTime.Now.AddDays(-1));
		(_, Project project3) = CreateTestProject();

		_context.Projects.Add(project1);
		_context.Projects.Add(project2);
		_context.Projects.Add(project3);

		await _context.SaveChangesAsync();

		// act
		int urnThatDoesNotExist = 12312;
		var projects = (await _subject.SearchProjects(null, null, null, 1, 10, urnThatDoesNotExist)).Item1.ToList();

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
			(ProjectDetails projectDetails, Project projectState) = CreateTestProject(DateTime.Now.AddDays(-i));
			createdProjects.Add(projectDetails);
			_context.Projects.Add(projectState);
		}

		await _context.SaveChangesAsync();

		// act
		var firstPage = (await _subject.SearchProjects(new string[] { }, null, null, 1, 2, null)).Item1.ToList();
		var secondPage = (await _subject.SearchProjects(new string[] { }, null, null, 2, 2, null)).Item1.ToList();
		var thirdPage = (await _subject.SearchProjects(new string[] { }, null, null, 3, 2, null)).Item1.ToList();

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

	private (ProjectDetails, Project) CreateTestProject(DateTime? applicationDate = null)
	{
		applicationDate ??= DateTime.Now;

		var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.ApplicationReceivedDate, applicationDate).Create();
		var newProject = new Project(0, projectDetails);

		return (projectDetails, newProject);
	}

	private Project AddProjectDetailToProject(ProjectDetails projectDetails)
	{
	    var newProject = new Project(0, projectDetails);
		return newProject;
	}
}


