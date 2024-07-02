using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectStatusesDataQueryTests
	{
		private readonly Fixture _fixture = new();

		private readonly ConversionProjectRepository _subject;
		private readonly AcademisationContext _context;
		private readonly IMediator _mediator;
		public ProjectStatusesDataQueryTests()
		{
			_context = new TestProjectContext(_mediator).CreateContext();
			_subject = new ConversionProjectRepository(_context, null);
		}

		[Fact]
		public async Task ProjectsExist__DuplicateStatuses__ReturnsDistinct()
		{
			var status = "Active";

			var projectDetails1 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, status).Create();

			var projectDetails2 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, status).Create();

			var projectDetails3 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, status).Create();

			_context.Projects.Add(new Project(1, projectDetails1));
			_context.Projects.Add(new Project(2, projectDetails2));
			_context.Projects.Add(new Project(3, projectDetails3));

			await _context.SaveChangesAsync();

			var results = await _subject.GetFilterParameters();

			Assert.Multiple(
				() => Assert.Equal(status, results.Statuses!.First()),
				() => Assert.Single(results.Statuses!)
			);
		}

		[Fact]
		public async Task NoProjects__ReturnsEmpty()
		{
			var results = await _subject.GetFilterParameters();

			Assert.Empty(results.Statuses!);
		}

		[Fact]
		public async Task ProjectsExist__ReturnsAlphabetically()
		{
			var projectDetails1 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, "InProgress").Create();

			var projectDetails2 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, "Closed").Create();

			var projectDetails3 = _fixture.Build<ProjectDetails>()
			.With(p => p.ProjectStatus, "Active").Create();

			_context.Projects.Add(new Project(1, projectDetails1));
			_context.Projects.Add(new Project(2, projectDetails2));
			_context.Projects.Add(new Project(3, projectDetails3));


			await _context.SaveChangesAsync();

			var results = await _subject.GetFilterParameters();

			Assert.Multiple(
				() => Assert.Equal(3, results.Statuses!.Count),
				() => Assert.Equal("Active", results.Statuses!.First()),
				() => Assert.Equal("Closed", results.Statuses![1]),
				() => Assert.Equal("InProgress", results.Statuses!.Last())
			);
		}
	}
}
