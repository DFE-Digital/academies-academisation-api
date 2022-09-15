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

	private readonly ProjectsListGetDataQuery _subject;
	private readonly AcademisationContext _context;

	public ProjectsListGetDataQueryTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ProjectsListGetDataQuery(_context);
	}

	[Fact]
	public async Task ProjectsExists_SearchProjectsByState__ReturnsProjects()
	{
		// arrange
		var (projectDetails1, projectState1) = CreateTestProject();
		var (_, projectState2) = CreateTestProject();
		var (_, projectState3) = CreateTestProject();

		_context.Projects.Add(projectState1);
		_context.Projects.Add(projectState2);
		_context.Projects.Add(projectState3);

		await _context.SaveChangesAsync();

		// act
		var searchStatus = new List<string> { projectState1!.ProjectStatus.ToLower() };
		var result = await _subject.SearchProjects(searchStatus, 1, 1, null);

		// assert
		var firstProject = result.ToList().FirstOrDefault();
		Assert.Multiple(
			() => Assert.NotNull(firstProject),
			() => Assert.Equal(firstProject!.Details, projectDetails1),
			() => Assert.Equal(projectState1.Id, firstProject!.Id)
		);
	}

	private Tuple<ProjectDetails, ProjectState> CreateTestProject()
	{
		var projectDetails = _fixture.Create<ProjectDetails>();
		var newProject = new Project(0, projectDetails);
		var mappedProject = ProjectState.MapFromDomain(newProject);

		return Tuple.Create(projectDetails, mappedProject);
	}
}
