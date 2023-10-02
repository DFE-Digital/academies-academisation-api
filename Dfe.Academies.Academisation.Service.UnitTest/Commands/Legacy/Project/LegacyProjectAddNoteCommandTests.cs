using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
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
		private readonly Fixture _fixture;
		private readonly Mock<IConversionProjectRepository> _projectGetDataQuery;
		private readonly Mock<IProjectNoteAddCommand> _projectNoteAddCommand;

		public LegacyProjectAddNoteCommandTests()
		{
			_fixture = new Fixture();
			_projectGetDataQuery = new Mock<IConversionProjectRepository>();
			_projectNoteAddCommand = new Mock<IProjectNoteAddCommand>();
		}

		private LegacyProjectAddNoteCommand System_under_test()
		{
			return new LegacyProjectAddNoteCommand(_projectGetDataQuery.Object, _projectNoteAddCommand.Object);
		}

		[Fact]
		public async Task Should_return_not_found_command_result_if_the_project_is_unknown()
		{
			_projectGetDataQuery
				.Setup(x => x.GetConversionProject(It.IsAny<int>()))
				.ReturnsAsync((IProject?)null);

			LegacyProjectAddNoteCommand command = System_under_test();

			CommandResult result = await command.Execute(_fixture.Create<LegacyProjectAddNoteModel>());

			result.Should().BeOfType<NotFoundCommandResult>();
		}

		[Fact]
		public async Task Should_pass_on_the_result_from_the_data_layer_command_if_the_project_is_known()
		{
			IProject project = _fixture.Create<Domain.ProjectAggregate.Project>();

			_projectGetDataQuery
				.Setup(x => x.GetConversionProject(It.IsAny<int>()))
				.ReturnsAsync(project);

			var addCommandResult = new CommandSuccessResult();
			_projectNoteAddCommand
				.Setup(x => x.Execute(It.IsAny<int>(), It.IsAny<ProjectNote>()))
				.ReturnsAsync(addCommandResult);

			LegacyProjectAddNoteCommand command = System_under_test();

			CommandResult result = await command.Execute(
				_fixture.Create<LegacyProjectAddNoteModel>() with { ProjectId = project.Id }
			);

			result.Should().Be(addCommandResult);
		}
	}
}
