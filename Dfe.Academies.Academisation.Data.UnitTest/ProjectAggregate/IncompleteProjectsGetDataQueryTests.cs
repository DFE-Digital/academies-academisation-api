using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class IncompleteProjectsGetDataQueryTests
	{
		private readonly Fixture _fixture = new();

		private readonly ConversionProjectRepository _subject;
		private readonly AcademisationContext _context;
		private Mock<IMapper> _mockMapper  = new Mock<IMapper>();

		public IncompleteProjectsGetDataQueryTests()
		{
			_context = new TestProjectContext().CreateContext();
			_subject = new ConversionProjectRepository(_context, _mockMapper.Object);
		}

		[Fact]
		public async Task ProjectExists___GetProject()
		{
			// arrange
			(ProjectDetails incompleteProjectDetails1, Project incompleteProjectState1) = CreateTestProject(region: "region1");
			(ProjectDetails incompleteProjectDetails2, Project incompleteProjectState2) = CreateTestProject("localAuth2");
			(_, Project completeProjectState3) = CreateTestProject("localAuth3", "region3");

			_context.Projects.Add(incompleteProjectState1);
			_context.Projects.Add(incompleteProjectState2);
			_context.Projects.Add(completeProjectState3);

			await _context.SaveChangesAsync();

			// act
			var result = (await _subject.GetIncompleteProjects())!.ToList();

			// assert
			Assert.Multiple(
				() => Assert.Equal(2, result.Count),
				() => Assert.True(incompleteProjectDetails1.Equals(result.First().Details)),
				() => Assert.True(incompleteProjectDetails2.Equals(result.Last().Details))
			);
		}

		private (ProjectDetails, Project) CreateTestProject(string? localAuthority = null, string? region = null)
		{
			var projectDetails = _fixture.Build<ProjectDetails>()
				.With(p => p.LocalAuthority, localAuthority)
				.With(p => p.Region, region)
				.Create();
			var newProject = new Project(0, projectDetails);

			return (projectDetails, newProject);
		}
	}
}
