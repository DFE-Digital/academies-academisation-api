using Dfe.Academies.Academisation.Core;
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
		private readonly Mock<IProjectGetDataQuery> _projectGetDataQuery;
		private readonly Mock<IProjectUpdateDataCommand> _projectUpdateDataCommand;
		private readonly LegacyProjectAddNoteModel _addNoteModel;

		public LegacyProjectAddNoteCommandTests()
		{
			_projectGetDataQuery = new Mock<IProjectGetDataQuery>();
			_projectUpdateDataCommand = new Mock<IProjectUpdateDataCommand>();
			_addNoteModel = new LegacyProjectAddNoteModel("Subject", "Note", "Author", DateTime.Today, 1234);
		}

		private LegacyProjectAddNoteCommand System_under_test()
		{
			return new LegacyProjectAddNoteCommand(_projectGetDataQuery.Object, _projectUpdateDataCommand.Object);
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

			_projectUpdateDataCommand.Verify(x => x.Execute(It.IsAny<IProject>()), Times.Never);

			result.Should().BeOfType<NotFoundCommandResult>();
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

			project.Details.Notes
				.Should().ContainEquivalentOf(new ProjectNote("Subject", "Note", "Author", _addNoteModel.Date));
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

			_projectUpdateDataCommand.Verify(x => x.Execute(project), Times.Once());

			result.Should().BeOfType<CommandSuccessResult>();
		}
	}
}
