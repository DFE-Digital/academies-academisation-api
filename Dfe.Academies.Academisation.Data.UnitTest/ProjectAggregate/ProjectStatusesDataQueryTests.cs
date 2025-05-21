using System;
using System.Globalization;
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
			_subject = new ConversionProjectRepository(_context);
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

		[Fact]
		public async Task ProjectsExist__ReturnsAdvisoryBoardDates_Sorted_Descending()
		{
			var projectDetails1 = _fixture.Build<ProjectDetails>()
				.With(p => p.HeadTeacherBoardDate, new DateTime(2023, 3, 4, 0, 0, 0, DateTimeKind.Utc)).Create();

			var projectDetails2 = _fixture.Build<ProjectDetails>()
				.With(p => p.HeadTeacherBoardDate, new DateTime(2026, 11, 19, 0, 0, 0, DateTimeKind.Utc)).Create();

			var projectDetails3 = _fixture.Build<ProjectDetails>()
				.With(p => p.HeadTeacherBoardDate, new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc)).Create();

			var projectDetails4 = _fixture.Build<ProjectDetails>()
				.With(p => p.HeadTeacherBoardDate, new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc)).Create();

			_context.Projects.Add(new Project(1, projectDetails1));
			_context.Projects.Add(new Project(2, projectDetails2));
			_context.Projects.Add(new Project(3, projectDetails3));
			_context.Projects.Add(new Project(4, projectDetails4));

			await _context.SaveChangesAsync();

			var results = await _subject.GetFilterParameters();

			// Assert
			Assert.True(results.AdvisoryBoardDates?.Count == 3);
			

			var date1 = DateTime.ParseExact(results.AdvisoryBoardDates![0], "MMM yy", CultureInfo.InvariantCulture);
			var date2 = DateTime.ParseExact(results.AdvisoryBoardDates![1], "MMM yy", CultureInfo.InvariantCulture);
			var date3 = DateTime.ParseExact(results.AdvisoryBoardDates![2], "MMM yy", CultureInfo.InvariantCulture);

			Assert.True(date1 > date2 && date2 > date3);
		}
	}
}
