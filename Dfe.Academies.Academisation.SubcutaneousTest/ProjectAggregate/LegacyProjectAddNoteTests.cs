using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate
{
	public class LegacyProjectAddNoteTests
	{
		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly Mock<IMediator> _mediator;
		private readonly Mock<IRequestHandler<ConversionProjectAddNoteCommand, CommandResult>> _projectAddNoteCommandHandler;
		private readonly IConversionProjectExportService _conversionProjectExportService;

		public LegacyProjectAddNoteTests()
		{

			_conversionProjectQueryService = Mock.Of<IConversionProjectQueryService>();
			_mediator = new Mock<IMediator>();
			_projectAddNoteCommandHandler = new Mock<IRequestHandler<ConversionProjectAddNoteCommand, CommandResult>>();
			_conversionProjectExportService = Mock.Of<IConversionProjectExportService>();
		}

		private ProjectController System_under_test()
		{
			return new ProjectController(
				_conversionProjectQueryService,
				_mediator.Object);
		}

		[Fact]
		public async Task Should_return_a_201_result_when_the_note_is_added_successfully()
		{
			_mediator
				.Setup(x => x.Send(It.IsAny<ConversionProjectAddNoteCommand>(), It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new CommandSuccessResult() as CommandResult));

			ProjectController controller = System_under_test();

			ActionResult result =
				await controller.AddNote(1234, new AddNoteRequest("Subject", "Note", "Author", DateTime.Today));

			result.Should().BeOfType<CreatedResult>();
		}

		[Fact]
		public async Task Should_return_a_404_result_if_the_id_does_not_match_a_known_project()
		{
			_mediator
				.Setup(x => x.Send(It.IsAny<ConversionProjectAddNoteCommand>(), It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new NotFoundCommandResult() as CommandResult));

			ProjectController controller = System_under_test();

			ActionResult result =
				await controller.AddNote(1234, new AddNoteRequest("Subject", "Note", "Author", DateTime.UtcNow));

			result.Should().BeOfType<NotFoundResult>();
		}
	}
}
