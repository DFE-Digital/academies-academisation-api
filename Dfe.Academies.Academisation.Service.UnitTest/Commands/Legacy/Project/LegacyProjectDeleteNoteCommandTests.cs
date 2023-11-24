using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class LegacyProjectDeleteNoteCommandTests
	{
		private readonly Mock<IProjectNoteDeleteCommand> _deleteNoteCommand;
		private readonly Fixture _fixture;

		public LegacyProjectDeleteNoteCommandTests()
		{
			_fixture = new Fixture();
			_deleteNoteCommand = new Mock<IProjectNoteDeleteCommand>();
		}

		private ConversionProjectDeleteNoteCommandHandler System_under_test()
		{
			return new ConversionProjectDeleteNoteCommandHandler(_deleteNoteCommand.Object);
		}

		[Fact]
		public async Task Should_pass_the_request_to_the_data_layer_command()
		{
			int projectId = Random.Shared.Next();
			var note = _fixture.Create<ConversionProjectDeleteNoteCommand>();

			var command = System_under_test();

			await command.Handle(note, default);

			_deleteNoteCommand.Verify(
				x => x.Execute(
					It.Is(projectId, EqualityComparer<int>.Default),
					It.Is<ProjectNote>(n => n.Subject == note.Subject &&
											n.Note == note.Note &&
											n.Author == note.Author &&
											n.Date == note.Date)
				)
			);
		}

		[Fact]
		public async Task Should_return_the_result_produced_by_the_data_layer_command()
		{
			var dataCommandResult = new CommandSuccessResult();

			_deleteNoteCommand
				.Setup(x => x.Execute(It.IsAny<int>(), It.IsAny<ProjectNote>()))
				.ReturnsAsync(dataCommandResult);

			var command = System_under_test();

			CommandResult result = await command.Handle(_fixture.Create<ConversionProjectDeleteNoteCommand>(), default);

			result.Should().Be(dataCommandResult);
		}
	}
}
