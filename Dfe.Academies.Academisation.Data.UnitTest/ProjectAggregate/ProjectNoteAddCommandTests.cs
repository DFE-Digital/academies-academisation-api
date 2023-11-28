using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using FluentAssertions;
using System.Threading.Tasks;
using System;
using Xunit;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectNoteAddCommandTests
	{
		private readonly ProjectNote _newNote;
		private readonly AcademisationContext _context;
		private readonly Fixture _fixture;

		public ProjectNoteAddCommandTests()
		{
			_fixture = new Fixture();
			Project project = _fixture.Create<Project>();

			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();

			_context.Projects.Add(project);
			_context.SaveChanges();

			_newNote = new ProjectNote(
				"Subject",
				"Note",
				"Author",
				DateTime.Today,
				project.Id)
			;
		}

		private ProjectNoteAddCommand System_under_test()
		{
			return new ProjectNoteAddCommand(_context);
		}

		[Fact]
		public async Task Should_return_not_found_result_if_the_specified_project_does_not_exist()
		{
			ProjectNote unknownNote = _fixture.Create<ProjectNote>();

			ProjectNoteAddCommand command = System_under_test();

			CommandResult result = await command.Execute(
				unknownNote.ProjectId,
				new ProjectNote(unknownNote.Subject, unknownNote.Note, unknownNote.Author, unknownNote.Date, unknownNote.ProjectId)
			);

			result.Should().BeOfType<NotFoundCommandResult>();

			_context.ProjectNotes.Should().NotContainEquivalentOf(_newNote
				, x => x.Excluding(q => q.Id));
		}

		[Fact]
		public async Task Should_add_the_new_note_to_the_project()
		{
			ProjectNoteAddCommand command = System_under_test();

			await command.Execute(
				_newNote.ProjectId,
				new ProjectNote(_newNote.Subject, _newNote.Note, _newNote.Author, _newNote.Date, _newNote.ProjectId)
			);

			_context.ProjectNotes.Should().ContainEquivalentOf(
				_newNote, x => x.Excluding(q => q.Id));
		}

		[Fact]
		public async Task Should_save_the_amended_project()
		{
			ProjectNoteAddCommand command = System_under_test();

			CommandResult result = await command.Execute(
				_newNote.ProjectId,
				new ProjectNote(_newNote.Subject, _newNote.Note, _newNote.Author, _newNote.Date, _newNote.ProjectId)
			);

			result.Should().BeOfType<CommandSuccessResult>();
		}
	}
}
