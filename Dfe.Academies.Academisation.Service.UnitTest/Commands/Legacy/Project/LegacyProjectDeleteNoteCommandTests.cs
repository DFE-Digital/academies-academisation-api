using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class LegacyProjectDeleteNoteCommandTests
	{
		private readonly AcademisationContext _context;
		private readonly Fixture _fixture;
		private readonly IList<ProjectNoteState> _notes;

		public LegacyProjectDeleteNoteCommandTests()
		{
			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();

			_fixture = new Fixture();

			IList<ProjectState> projects = _fixture.CreateMany<ProjectState>(3).ToList();

			_notes = _fixture.CreateMany<ProjectNoteState>()
				.Select(x =>
				{
					x.ProjectId = projects[Random.Shared.Next(projects.Count)].Id;
					return x;
				})
				.ToList();

			_context.Projects.AddRangeAsync(projects);
			_context.ProjectNotes.AddRangeAsync(_notes);
			_context.SaveChanges();
		}

		private ProjectNoteState RandomNote => _notes[Random.Shared.Next(_notes.Count)];

		private LegacyProjectDeleteNoteCommand System_under_test()
		{
			return new LegacyProjectDeleteNoteCommand(_context);
		}

		private static ProjectNoteServiceModel NoteModelFrom(ProjectNoteState noteState)
		{
			return new ProjectNoteServiceModel
			{
				Author = noteState.Author, Subject = noteState.Subject, Note = noteState.Note, Date = noteState.Date
			};
		}

		[Fact]
		public async Task Should_return_not_found_result_if_the_note_does_not_exist()
		{
			ProjectNoteState noteToDelete = _fixture.Create<ProjectNoteState>();
			ProjectNoteServiceModel noteModel = NoteModelFrom(noteToDelete);

			LegacyProjectDeleteNoteCommand command = System_under_test();

			_context.ProjectNotes.Should().NotContain(noteToDelete);

			CommandResult result = await command.Execute(noteToDelete.ProjectId, noteModel);

			result.Should().BeOfType<NotFoundCommandResult>();
		}

		[Fact]
		public async Task Should_remove_the_note()
		{
			ProjectNoteState noteToDelete = RandomNote;
			ProjectNoteServiceModel noteModel = NoteModelFrom(noteToDelete);

			LegacyProjectDeleteNoteCommand command = System_under_test();

			_context.ProjectNotes.Should().Contain(noteToDelete);

			CommandResult result = await command.Execute(noteToDelete.ProjectId, noteModel);

			result.Should().BeOfType<CommandSuccessResult>();

			_context.ProjectNotes.Should().NotContain(noteToDelete);
		}
	}
}
