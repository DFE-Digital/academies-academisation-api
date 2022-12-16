using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate
{
	public class LegacyProjectAddNoteTests
	{
		private readonly IProjectGetStatusesQuery _getStatusesQuery;
		private readonly Mock<ILegacyProjectAddNoteCommand> _projectAddNoteCommand;
		private readonly ILegacyProjectGetQuery _projectGetQuery;
		private readonly ILegacyProjectListGetQuery _projectListGetQuery;
		private readonly ILegacyProjectUpdateCommand _projectUpdateCommand;

		public LegacyProjectAddNoteTests()
		{
			_projectGetQuery = Mock.Of<ILegacyProjectGetQuery>();
			_projectListGetQuery = Mock.Of<ILegacyProjectListGetQuery>();
			_getStatusesQuery = Mock.Of<IProjectGetStatusesQuery>();
			_projectUpdateCommand = Mock.Of<ILegacyProjectUpdateCommand>();
			_projectAddNoteCommand = new Mock<ILegacyProjectAddNoteCommand>();
		}

		private LegacyProjectController System_under_test()
		{
			return new LegacyProjectController(
				_projectGetQuery,
				_projectListGetQuery,
				_getStatusesQuery,
				_projectUpdateCommand,
				_projectAddNoteCommand.Object);
		}

		[Fact]
		public async Task Should_return_a_201_result_when_the_note_is_added_successfully()
		{
			_projectAddNoteCommand
				.Setup(x => x.Execute(It.IsAny<LegacyProjectAddNoteModel>()))
				.Returns(Task.FromResult(new CommandSuccessResult() as CommandResult));

			LegacyProjectController controller = System_under_test();

			ActionResult result =
				await controller.AddNote(1234, new AddNoteRequest("Subject", "Note", "Author", DateTime.Today));

			result.Should().BeOfType<CreatedResult>();
		}

		[Fact]
		public async Task Should_return_a_404_result_if_the_id_does_not_match_a_known_project()
		{
			_projectAddNoteCommand
				.Setup(x => x.Execute(It.IsAny<LegacyProjectAddNoteModel>()))
				.Returns(Task.FromResult(new NotFoundCommandResult() as CommandResult));

			LegacyProjectController controller = System_under_test();

			ActionResult result =
				await controller.AddNote(1234, new AddNoteRequest("Subject", "Note", "Author", DateTime.UtcNow));

			result.Should().BeOfType<NotFoundResult>();
		}
	}
}
