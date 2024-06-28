using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommandTests
	{
		private readonly Fixture _fixture;
		private readonly Mock<IConversionProjectRepository> _repo;

		public LegacyProjectAddNoteCommandTests()
		{
			_fixture = new Fixture();
			_repo = new Mock<IConversionProjectRepository>();
			_repo.Setup(x => x.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
		}

		private ConversionProjectAddNoteCommandHandler System_under_test()
		{
			return new ConversionProjectAddNoteCommandHandler(_repo.Object);
		}

		[Fact]
		public async Task Should_return_not_found_command_result_if_the_project_is_unknown()
		{
			_repo
				.Setup(x => x.GetConversionProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((IProject?)null);

			var command = System_under_test();

			CommandResult result = await command.Handle(_fixture.Create<ConversionProjectAddNoteCommand>(), default);

			result.Should().BeOfType<NotFoundCommandResult>();
		}

		[Fact]
		public async Task Should_pass_on_the_result_from_the_data_layer_command_if_the_project_is_known()
		{
			IProject project = _fixture.Create<Domain.ProjectAggregate.Project>();

			_repo.Setup(x => x.GetConversionProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(project);

			var command = System_under_test();

			CommandResult result = await command.Handle(
				_fixture.Create<ConversionProjectAddNoteCommand>() with { ProjectId = project.Id }, default
			);

			result.Should().BeOfType(typeof(CommandSuccessResult));
		}
	}
}
