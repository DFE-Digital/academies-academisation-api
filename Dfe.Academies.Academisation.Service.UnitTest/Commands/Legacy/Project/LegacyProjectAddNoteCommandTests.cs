using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommandTests
	{
		private readonly LegacyProjectAddNoteModel _addNoteModel;
		private readonly AcademisationContext _context;
		private readonly Mock<IProjectGetDataQuery> _projectGetDataQuery;

		public LegacyProjectAddNoteCommandTests()
		{
			ProjectState projectState = new Fixture().Create<ProjectState>();

			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();

			_context.Projects.Add(projectState);
			_context.SaveChanges();

			_projectGetDataQuery = new Mock<IProjectGetDataQuery>();
			_addNoteModel = new LegacyProjectAddNoteModel(
				"Subject",
				"Note",
				"Author",
				DateTime.Today,
				projectState.Id
			);
		}

		private LegacyProjectAddNoteCommand System_under_test()
		{
			return new LegacyProjectAddNoteCommand(
				_projectGetDataQuery.Object,
				_context
			);
		}

		[Fact]
		public async Task Should_return_not_found_result_if_the_specified_project_does_not_exist()
		{
			_projectGetDataQuery
				.Setup(x => x.Execute(It.IsAny<int>()))
				.Returns(Task.FromResult(null as IProject));

			LegacyProjectAddNoteCommand command = System_under_test();

			CommandResult result =
				await command.Execute(_addNoteModel);

			result.Should().BeOfType<NotFoundCommandResult>();

			_context.ProjectNotes.Should().NotContainEquivalentOf(
				new ProjectNoteState
				{
					Date = _addNoteModel.Date,
					Subject = _addNoteModel.Subject,
					Author = _addNoteModel.Author,
					Note = _addNoteModel.Note,
					ProjectId = _addNoteModel.ProjectId
				}, x => x.Excluding(q => q.Id));
		}

		[Fact]
		public async Task Should_add_the_new_note_to_the_project()
		{
			var project = new Domain.ProjectAggregate.Project(_addNoteModel.ProjectId, new ProjectDetails());

			_projectGetDataQuery
				.Setup(x => x.Execute(It.IsAny<int>()))
				.Returns(Task.FromResult(project as IProject)!);

			LegacyProjectAddNoteCommand command = System_under_test();

			await command.Execute(_addNoteModel with { ProjectId = project.Id });

			_context.ProjectNotes.Should().ContainEquivalentOf(
				new ProjectNoteState
				{
					Date = _addNoteModel.Date,
					Subject = _addNoteModel.Subject,
					Author = _addNoteModel.Author,
					Note = _addNoteModel.Note,
					ProjectId = _addNoteModel.ProjectId
				}, x => x.Excluding(q => q.Id));
		}

		[Fact]
		public async Task Should_save_the_amended_project()
		{
			var project = new Domain.ProjectAggregate.Project(_addNoteModel.ProjectId, new ProjectDetails());

			_projectGetDataQuery
				.Setup(x => x.Execute(It.IsAny<int>()))
				.Returns(Task.FromResult(project as IProject)!);

			LegacyProjectAddNoteCommand command = System_under_test();

			CommandResult result = await command.Execute(_addNoteModel with { ProjectId = project.Id });

			result.Should().BeOfType<CommandSuccessResult>();
		}
	}
}
