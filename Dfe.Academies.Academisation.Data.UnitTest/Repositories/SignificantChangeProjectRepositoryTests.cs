using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.Repositories
{
	public class SignificantChangeProjectRepositoryTests
	{
		private readonly Fixture _fixture = new();
		private readonly AcademisationContext _context;
		private readonly SignificantChangeProjectRepository _repository;
		private readonly IMediator _mediator = new Mock<IMediator>().Object;

		public SignificantChangeProjectRepositoryTests()
		{
			_context = new TestProjectContext(_mediator).CreateContext();
			_repository = new SignificantChangeProjectRepository(_context);
		}

		[Fact]
		public async Task GetSignificantChangeProjectById_ProjectExists_ReturnsProject()
		{
			var project = _fixture.Create<SignificantChangeProject>();
			_context.SignificantChangeProjects.Add(project);
			await _context.SaveChangesAsync();

			var result = await _repository.GetSignificantChangeProjectById(project.Id, CancellationToken.None);

			result.Should().NotBeNull();
			result!.Id.Should().Be(project.Id);
		}

		[Fact]
		public async Task GetSignificantChangeProjectById_ProjectDoesNotExist_ReturnsNull()
		{
			var result = await _repository.GetSignificantChangeProjectById(999, CancellationToken.None);

			result.Should().BeNull();
		}

		[Fact]
		public async Task SearchSignificantChangeProjects_NoFilters_ReturnsAllProjects()
		{
			var projects = _fixture.CreateMany<SignificantChangeProject>(5).ToList();
			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, null, null, null, null, null,
					CancellationToken.None);

			totalCount.Should().Be(5);
			resultProjects.Should().HaveCount(5);
		}

		[Fact]
		public async Task SearchSignificantChangeProjects_FilterByStatus_ReturnsMatchingProjects()
		{
			var projects = new List<SignificantChangeProject>
			{
				new SignificantChangeProject(SignificantChangeStatus.PreDecision, 2222, 1, "trust", "66666", "route", "Test School"),
				new SignificantChangeProject(SignificantChangeStatus.PreDecision, 3333, 1, "Test Trust", "77777", "route", "school"),
				new SignificantChangeProject(SignificantChangeStatus.Approved, 4444, 1, "trust", "99999", "route", "school")
			};

			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			var statuses = new List<string> { "predecision" };

			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, null, statuses, null, null, null, CancellationToken.None);

			totalCount.Should().Be(2);
			resultProjects.Should().OnlyContain(p => p.Status == SignificantChangeStatus.PreDecision);
		}

		[Theory]
		[InlineData("TEST", 2)]
		[InlineData("test", 2)]
		[InlineData("77", 1)]
		[InlineData("school", 3)]
		[InlineData("SCHOOL", 3)]
		[InlineData("44", 1)]
		public async Task SearchSignificantChangeProjects_FilterByKeyword_ReturnsMatchingProjects(string keyword,
			int expectedNumberOfResults)
		{
			var projects = new List<SignificantChangeProject>
			{
				SignificantChangeProject.Create(2222, 1, "trust", "66666", "route", "Test School", DateTime.UtcNow),
				SignificantChangeProject.Create(3333, 1, "Test Trust", "77777", "route", "school", DateTime.UtcNow),
				SignificantChangeProject.Create(4444, 1, "trust", "99999", "route", "school", DateTime.UtcNow)
			};
			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, keyword, null, null, null, null,
					CancellationToken.None);

			totalCount.Should().Be(expectedNumberOfResults);
		}

		[Fact]
		public async Task SearchSignificantChangeProjects_FilterByAssignee_ReturnsMatchingProjects()
		{
			var projects = new List<SignificantChangeProject>
			{
				_fixture.Build<SignificantChangeProject>().Create(),
				_fixture.Build<SignificantChangeProject>().Create(),
				_fixture.Build<SignificantChangeProject>().Create()
			};

			projects[0].AssignUser(Guid.NewGuid(), "t@t.com", "John Doe");
			projects[1].AssignUser(Guid.NewGuid(), "t@t.com", "Jane Doe");
			projects[2].AssignUser(Guid.NewGuid(), "t@t.com", string.Empty);

			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			var assignees = new List<string> { "john doe" };

			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, null, null, assignees, null, null,
					CancellationToken.None);


			totalCount.Should().Be(1);
			resultProjects.Should().ContainSingle(p => p.AssignedUserFullName == "John Doe");
		}

		[Fact]
		public async Task SearchSignificantChangeProjects_FilterByAssignee_NotAssigned_ReturnsMatchingProjects()
		{
			var projects = new List<SignificantChangeProject>
			{
				_fixture.Build<SignificantChangeProject>().Create(),
				_fixture.Build<SignificantChangeProject>().Create(),
				_fixture.Build<SignificantChangeProject>().Create(),
				_fixture.Build<SignificantChangeProject>().Create()
			};

			projects[0].AssignUser(Guid.NewGuid(), "t@t.com", "John Doe");
			projects[1].AssignUser(Guid.NewGuid(), "t@t.com", "Jane Doe");
			projects[2].AssignUser(Guid.NewGuid(), "t@t.com", string.Empty);

			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			var assignees = new List<string> { "John Doe", "Not Assigned" };

			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, null, null, assignees, null, null,
					CancellationToken.None);

			totalCount.Should().Be(3);
			resultProjects.Should().Contain(p => p.AssignedUserFullName == "John Doe");
			resultProjects.Should().Contain(p => string.IsNullOrEmpty(p.AssignedUserFullName));
		}

		[Fact]
		public async Task SearchSignificantChangeProjects_Pagination_ReturnsCorrectPage()
		{
			// Arrange
			var projects = _fixture.CreateMany<SignificantChangeProject>(20).ToList();
			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();

			// Act
			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(2, 5, null, null, null, null, null,
					CancellationToken.None);

			// Assert
			totalCount.Should().Be(20);
			resultProjects.Should().HaveCount(5);
		}

		[Theory]
		[InlineData("Sponsored", "Converter")]
		[InlineData("SPONSORED", "CONVERTER")]
		public async Task SearchSignificantChangeProjects_FilterByRoute_ReturnsMatchingProjects(params string[] routes)
		{
			var projects = new List<SignificantChangeProject>
			{
				SignificantChangeProject.Create(2222, 1, "trust", "66666", "Sponsored", "Test School",
					DateTime.UtcNow),
				SignificantChangeProject.Create(3333, 1, "Test Trust", "77777", "Converter", "school",
					DateTime.UtcNow),
				SignificantChangeProject.Create(4444, 1, "trust", "99999", "Form a MAT", "school", DateTime.UtcNow)
			};

			_context.SignificantChangeProjects.AddRange(projects);
			await _context.SaveChangesAsync();


			var (resultProjects, totalCount) =
				await _repository.SearchSignificantChangeProjects(1, 10, null, null, null, null, routes.ToList(),
					CancellationToken.None);

			totalCount.Should().Be(2);
			resultProjects.Should().OnlyContain(p => routes.Contains(p.TypeOfSignificantChange, StringComparer.InvariantCultureIgnoreCase));
		}
	}
}
