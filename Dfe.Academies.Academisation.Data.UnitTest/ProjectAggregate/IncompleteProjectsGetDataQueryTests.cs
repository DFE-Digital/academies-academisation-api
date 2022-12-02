using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class IncompleteProjectsGetDataQueryTests
	{
		private readonly Fixture _fixture = new();

		private readonly IncompleteProjectsGetDataQuery _subject;
		private readonly AcademisationContext _context;

		public IncompleteProjectsGetDataQueryTests()
		{
			_context = new TestProjectContext().CreateContext();
			_subject = new IncompleteProjectsGetDataQuery(_context);
		}

		[Fact]
		public async Task ProjectExists___GetProject()
		{
			// arrange
			(ProjectDetails projectDetails1, ProjectState projectState1) = CreateTestProject(region: "region1");
			(ProjectDetails projectDetails2, ProjectState projectState2) = CreateTestProject("localAuth2");
			(_, ProjectState projectState3) = CreateTestProject("localAuth3", "region3");

			_context.Projects.Add(projectState1);
			_context.Projects.Add(projectState2);
			_context.Projects.Add(projectState3);

			await _context.SaveChangesAsync();

			// act
			var result = (await _subject.GetIncompleteProjects())!.ToList();

			// assert
			Assert.Multiple(
				() => Assert.Equal(2, result.Count),
				() => Assert.Equivalent(projectDetails1, result.First().Details),
				() => Assert.Equivalent(projectDetails2, result.Last().Details)
			);
		}

		private (ProjectDetails, ProjectState) CreateTestProject(string? localAuthority = null, string? region = null)
		{
			var projectDetails = _fixture.Build<ProjectDetails>()
				.With(p => p.LocalAuthority, localAuthority)
				.With(p => p.Region, region)
				.Create();
			var newProject = new Project(0, projectDetails);
			var mappedProject = ProjectState.MapFromDomain(newProject);

			return (projectDetails, mappedProject);
		}
	}
}
