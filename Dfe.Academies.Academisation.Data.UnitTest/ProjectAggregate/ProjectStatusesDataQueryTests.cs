using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectStatusesDataQueryTests
	{
		private readonly Fixture _fixture = new();

		private readonly ProjectStatusesDataQuery _subject;
		private readonly AcademisationContext _context;

		public ProjectStatusesDataQueryTests()
		{
			_context = new TestProjectContext().CreateContext();
			_subject = new ProjectStatusesDataQuery(_context);
		}

		[Fact]
		public async Task ProjectsExist__DuplicateStatuses__ReturnsDistinct()
		{
			var status = "Active";
			_context.Projects.Add(new ProjectState() { ProjectStatus = status });
			_context.Projects.Add(new ProjectState() { ProjectStatus = status });
			_context.Projects.Add(new ProjectState() { ProjectStatus = status });

			await _context.SaveChangesAsync();

			var results = await _subject.Execute();

			Assert.Multiple(
				() => Assert.Equal(status, results.Statuses.First()),
				() => Assert.Single(results.Statuses)
			);
		}

		[Fact]
		public async Task NoProjects__ReturnsEmpty()
		{
			var results = await _subject.Execute();

			Assert.Empty(results.Statuses);
		}

		[Fact]
		public async Task ProjectsExist__ReturnsAlphabetically()
		{
			_context.Projects.Add(new ProjectState() { ProjectStatus = "InProgress" });
			_context.Projects.Add(new ProjectState() { ProjectStatus = "Closed" });
			_context.Projects.Add(new ProjectState() { ProjectStatus = "Active" });


			await _context.SaveChangesAsync();

			var results = await _subject.Execute();

			Assert.Multiple(
				() => Assert.Equal(3, results.Statuses.Count),
				() => Assert.Equal("Active", results.Statuses.First()),
				() => Assert.Equal("Closed", results.Statuses[1]),
				() => Assert.Equal("InProgress", results.Statuses.Last())
			);
		}
	}
}
