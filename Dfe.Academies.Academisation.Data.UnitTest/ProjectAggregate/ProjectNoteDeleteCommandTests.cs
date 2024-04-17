using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectNoteDeleteCommandTests
	{
		private readonly AcademisationContext _context;
		private readonly Fixture _fixture;
		private readonly IList<ProjectNote> _notes;

		public ProjectNoteDeleteCommandTests()
		{
			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();

			_fixture = new Fixture();

			IList<Project> projects = _fixture.CreateMany<Project>(3).ToList();

			_notes = _fixture.CreateMany<ProjectNote>().ToList();

			_notes = _notes.Select(x => new ProjectNote(x.Subject, x.Note, x.Author, x.Date, projects[Random.Shared.Next(projects.Count)].Id))
				.ToList();

			_context.Projects.AddRangeAsync(projects);
			_context.ProjectNotes.AddRangeAsync(_notes);
			_context.SaveChanges();
		}

		private ProjectNote RandomNote => _notes[Random.Shared.Next(_notes.Count)];

		private ProjectNoteDeleteCommand System_under_test()
		{
			return new ProjectNoteDeleteCommand(_context);
		}

		[Fact]
		public async Task Should_return_not_found_result_if_the_note_does_not_exist()
		{
			ProjectNote noteToDelete = _fixture.Create<ProjectNote>();

			ProjectNoteDeleteCommand command = System_under_test();

			_context.ProjectNotes.Should().NotContain(noteToDelete);

			CommandResult result = await command.Execute(noteToDelete.ProjectId, noteToDelete);

			result.Should().BeOfType<NotFoundCommandResult>();
		}

		[Fact]
		public async Task Should_remove_the_note()
		{
			ProjectNote noteToDelete = RandomNote;

			ProjectNoteDeleteCommand command = System_under_test();

			_context.ProjectNotes.Should().Contain(noteToDelete);

			CommandResult result = await command.Execute(noteToDelete.ProjectId, noteToDelete);

			result.Should().BeOfType<CommandSuccessResult>();

			_context.ProjectNotes.Should().NotContain(noteToDelete);
		}
	}
}
