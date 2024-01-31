using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectsListGetDataQueryTests
{
	private readonly Fixture _fixture = new();

	private readonly ConversionProjectRepository _subject;
	private readonly AcademisationContext _context;

	public ProjectsListGetDataQueryTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ConversionProjectRepository(_context, null);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByStatus__ReturnsProjects()
	{
		// arrange
		(_, Project project1) = CreateTestProject(null);
		(_, Project project2) = CreateTestProject(null);
		(_, Project project3) = CreateTestProject();

		var projectDetails1 = _fixture.Build<ProjectDetails>().Create();
		var projectDetails2 = _fixture.Build<ProjectDetails>().Create();



		project1 = AddProjectDetailToProject(projectDetails1, DateTime.Now.AddDays(-1));
		project2 = AddProjectDetailToProject(projectDetails2, DateTime.Now.AddDays(-2));


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
	public async Task ProjectsExists_SearchProjectsV2ByStatus__ReturnsProjects()
	{
		// arrange
		(_, Project project1) = CreateTestProject(null);
		(_, Project project2) = CreateTestProject(null);
		(_, Project project3) = CreateTestProject();

		var projectDetails1 = _fixture.Build<ProjectDetails>().Create();
		var projectDetails2 = _fixture.Build<ProjectDetails>().Create();

		project1 = AddProjectDetailToProject(projectDetails1, DateTime.Now.AddDays(-1));
		project2 = AddProjectDetailToProject(projectDetails2, DateTime.Now.AddDays(-2));

		_context.Projects.Add(project1);
		_context.Projects.Add(project2);
		_context.Projects.Add(project3);

		await _context.SaveChangesAsync();

		// act
		var searchStatus =
			new string[] { project1.Details.ProjectStatus!.ToLower(), project2.Details.ProjectStatus!.ToLower() };
		var projects = (await _subject.SearchProjectsV2(searchStatus, null, null, null, null, null, 1, 10)).Item1.ToList();

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


	// TODO:EA finish this unit test
	//[Fact]
	//public async Task ProjectsExists_SearchMATProjects__ReturnsMATProjects()
	//{
	//	// arrange
	//	(_, Project project1) = CreateTestProject(null);
	//	(_, Project project2) = CreateTestProject(null);
	//	(_, Project project3) = CreateTestProject();

	//	var projectDetails1 = _fixture.Build<ProjectDetails>()
	//		.With(p => p.AcademyTypeAndRoute, "TEST EAAAAAA")
	//		.Create();
	//	var projectDetails2 = _fixture.Build<ProjectDetails>().Create();

	//	project1 = AddProjectDetailToProject(projectDetails1, DateTime.Now.AddDays(-1));
	//	project2 = AddProjectDetailToProject(projectDetails2, DateTime.Now.AddDays(-2));

	//	_context.Projects.Add(project1);
	//	_context.Projects.Add(project2);
	//	_context.Projects.Add(project3);

	//	await _context.SaveChangesAsync();

	//	// act
	//	var projects = (await _subject.SearchMATProjects(null, null, null, null, null, null, 1, 10)).Item1.ToList();

	//	// assert
	//	var firstProject = projects.FirstOrDefault();
	//	var secondProject = projects.LastOrDefault();

	//	Assert.Multiple(
	//		() => Assert.NotNull(firstProject),
	//		() => Assert.Equal(firstProject!.Details, projectDetails1),
	//		() => Assert.Equal(project1.Id, firstProject!.Id),
	//		() => Assert.NotNull(secondProject),
	//		() => Assert.Equal(secondProject!.Details, projectDetails2),
	//		() => Assert.Equal(project2.Id, secondProject!.Id)
	//	);
	//}


	[Fact]
	public async Task ProjectsExists_SearchProjectsByTitle__ReturnsProjects()
	{
		// arrange
		string searchTerm = "Bristol";
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(null);
			if (i % 2 == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.SchoolName,$"{searchTerm} {i}" )
			.Create();


				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
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
	public async Task ProjectsExists_SearchProjectsV2ByTitle__ReturnsProjects()
	{
		// arrange
		string searchTerm = "Bristol";
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(null);
			if (i % 2 == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.SchoolName, $"{searchTerm} {i}")
			.Create();


				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, searchTerm, null, null, null, null, 1, 10)).Item1.ToList();

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
			(_, Project project) = CreateTestProject();

			User user = new User(Guid.NewGuid(), deliveryOfficer, "dave@dave.com");
			if (i % 2 == 0) 
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, user).Create();

				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
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
	public async Task ProjectsExists_SearchProjectsV2ByDeliveryOfficer__ReturnsProjects()
	{
		// arrange
		string deliveryOfficer = "Dave";
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject();

			User user = new User(Guid.NewGuid(), deliveryOfficer, "dave@dave.com");
			if (i % 2 == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, user).Create();

				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, null, new[] { deliveryOfficer }, null, null, null, 1, 10)).Item1.ToList();

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
			(_, Project project) = CreateTestProject(null);
			if (i < 2) 
			{

			User user = new User(Guid.NewGuid(), deliveryOfficers[i], "dave@dave.com");
			
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, user).Create();

			project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));	
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
	public async Task ProjectsExists_SearchProjectsV2ByMultipleDeliveryOfficers__ReturnsProjects()
	{
		// arrange
		string[] deliveryOfficers = new[] { "Dave", "Bob" };
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(null);
			if (i < 2)
			{

				User user = new User(Guid.NewGuid(), deliveryOfficers[i], "dave@dave.com");

				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, user).Create();

				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, null, deliveryOfficers, null, null, null, 1, 10)).Item1.ToList();

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
			(_, Project project) = CreateTestProject(null);
			if (i < 2)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.Region, regions[i])
			.Create();

			project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));	 
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
	public async Task ProjectsExists_SearchProjectsV2ByMultipleRegions__ReturnsProjects()
	{
		// arrange
		string[] regions = { "east", "west" };
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(null);
			if (i < 2)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.Region, regions[i])
			.Create();

				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, null, null, regions, null, null, 1, 10)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(2, projects.Count),
			() => Assert.Equal(regions[0], projects[0].Details.Region),
			() => Assert.Equal(regions[1], projects[1].Details.Region)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsV2ByMultipleLocalAuthorities__ReturnsProjects()
	{
		// arrange
		string[] localAuthorities = { "sheffield", "Bradford" };
		for (int i = 0; i < 6; i++)
		{
			(_, Project project) = CreateTestProject(null);
			if (i < 2)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.LocalAuthority, localAuthorities[i])
			.Create();

				project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			}
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, null, null, null, localAuthorities, null, 1, 10)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(2, projects.Count),
			() => Assert.Equal(localAuthorities[0], projects[0].Details.LocalAuthority),
			() => Assert.Equal(localAuthorities[1], projects[1].Details.LocalAuthority)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByAllCriteria__ReturnsProject()
	{
		// arrange
		var ( deliveryOfficer, status, title, urn ) = ( "Dave", "active", "school", 1234 );

		User user = new User(Guid.NewGuid(), deliveryOfficer,"dave@dave.com");

		for (int i = 0; i < 3; i++)
		{
			(_, Project project) = CreateTestProject(null);



			if (i == 0)
			{
				var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, user)
			.With(p => p.ProjectStatus, status)
			.With(p => p.SchoolName, title)
			.With(p => p.Urn, urn)
			.Create();

			project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));

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
			() => Assert.Equal(projects[0].Details.AssignedUser, user),
			() => Assert.Equal(projects[0].Details.ProjectStatus, status),
			() => Assert.Equal(projects[0].Details.SchoolName, title),
			() => Assert.Equal(projects[0].Details.Urn, urn)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchUnassignedProjects__ReturnsProjects()
	{
		// arrange
		User user = new User(Guid.NewGuid(), "Dave", "dave@dave.com");
		User emptyUser1 = new User(Guid.NewGuid(),"", "dave@dave1.com");
		User emptyUser2 = new User(Guid.NewGuid(), "", "dave@dave2.com");
		string[] deliveryOfficers = new[] { "Dave", "Not assigned" };
		
		(_, Project project1) = CreateTestProject(null);
		var projectDetails1 = _fixture.Build<ProjectDetails>()
		.With(p => p.AssignedUser, user).Create();
			project1 = AddProjectDetailToProject(projectDetails1, DateTime.Now);
		
		
		(_, Project project2) = CreateTestProject(null);
		     var projectDetails2 = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser,emptyUser1).Create();
		project2 = AddProjectDetailToProject(projectDetails2, DateTime.Now.AddDays(-1));

		(_, Project project3) = CreateTestProject(null);
	        var projectDetails3 = _fixture.Build<ProjectDetails>()
			.With(p => p.AssignedUser, emptyUser2).Create();
		project3 = AddProjectDetailToProject(projectDetails3, DateTime.Now.AddDays(-2));

		(_, Project project4) = CreateTestProject(null);
		var projectDetails4 = _fixture.Build<ProjectDetails>().Create();
		project4 = AddProjectDetailToProject(projectDetails4, DateTime.Now.AddDays(-3));

		await _context.Projects.AddRangeAsync(project1, project2, project3, project4);	
		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjects(null, null, deliveryOfficers, 1, 10, null)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal(deliveryOfficers[0], projects[0].Details.AssignedUser!.FullName),
			() => Assert.Equal("", projects[1].Details.AssignedUser?.FullName),
			() => Assert.Equal("", projects[2].Details.AssignedUser?.FullName)
		);
	}

	[Fact]
	public async Task ProjectsExists_SearchUnassignedProjectsV2__ReturnsProjects()
	{
		// arrange
		User user = new User(Guid.NewGuid(), "Dave", "dave@dave.com");
		User emptyUser1 = new User(Guid.NewGuid(), "", "dave@dave1.com");
		User emptyUser2 = new User(Guid.NewGuid(), "", "dave@dave2.com");
		string[] deliveryOfficers = new[] { "Dave", "Not assigned" };

		(_, Project project1) = CreateTestProject(null);
		var projectDetails1 = _fixture.Build<ProjectDetails>()
		.With(p => p.AssignedUser, user).Create();
		project1 = AddProjectDetailToProject(projectDetails1, DateTime.Now);


		(_, Project project2) = CreateTestProject(null);
		var projectDetails2 = _fixture.Build<ProjectDetails>()
	   .With(p => p.AssignedUser, emptyUser1).Create();
		project2 = AddProjectDetailToProject(projectDetails2, DateTime.Now.AddDays(-1));

		(_, Project project3) = CreateTestProject(null);
		var projectDetails3 = _fixture.Build<ProjectDetails>()
		.With(p => p.AssignedUser, emptyUser2).Create();
		project3 = AddProjectDetailToProject(projectDetails3, DateTime.Now.AddDays(-2));

		(_, Project project4) = CreateTestProject(null);
		var projectDetails4 = _fixture.Build<ProjectDetails>().Create();
		project4 = AddProjectDetailToProject(projectDetails4, DateTime.Now.AddDays(-3));

		await _context.Projects.AddRangeAsync(project1, project2, project3, project4);
		await _context.SaveChangesAsync();

		// act		
		var projects = (await _subject.SearchProjectsV2(null, null, deliveryOfficers, null, null, null, 1, 10)).Item1.ToList();

		// assert		
		Assert.Multiple(
			() => Assert.Equal(3, projects.Count),
			() => Assert.Equal(deliveryOfficers[0], projects[0].Details.AssignedUser!.FullName),
			() => Assert.Equal("", projects[1].Details.AssignedUser?.FullName),
			() => Assert.Equal("", projects[2].Details.AssignedUser?.FullName)
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
	public async Task ProjectsExists__GetPagedProjectsV2__ReturnsTotalCount()
	{
		// arrange
		for (int i = 0; i < 6; i++)
		{
			(ProjectDetails projectDetails, Project project) = CreateTestProject(DateTime.Now.AddDays(-i));
			_context.Projects.Add(project);
		}

		await _context.SaveChangesAsync();

		// act
		var (firstPageProjects, firstPageCount) = await _subject.SearchProjectsV2(new string[] { }, null, null, null, null, null, 1, 2);
		var (secondPageProjects, secondPageCount) = await _subject.SearchProjectsV2(new string[] { }, null, null, null, null, null, 2, 2);

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
			(_, Project project) = CreateTestProject(null);

			var projectDetails = _fixture.Build<ProjectDetails>().Create();

			createdProjects.Add(projectDetails);

			project = AddProjectDetailToProject(projectDetails, DateTime.Now.AddDays(-i));
			_context.Projects.Add(project);
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

	private (ProjectDetails, Project) CreateTestProject(DateTime? applicationDate = null, DateTime? createdOnDate = null)
	{
		applicationDate ??= DateTime.Now;

		var projectDetails = _fixture.Build<ProjectDetails>()
			.With(p => p.ApplicationReceivedDate, applicationDate)
			.Create();
		var newProject = new Project(0, projectDetails, createdOnDate);

		return (projectDetails, newProject);
	}

	private Project AddProjectDetailToProject(ProjectDetails projectDetails, DateTime createdOnDate)
	{
	    var newProject = new Project(0, projectDetails, createdOnDate);
		return newProject;
	}
}


