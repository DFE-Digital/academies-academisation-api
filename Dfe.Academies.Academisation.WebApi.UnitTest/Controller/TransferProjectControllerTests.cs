using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
    public class TransferProjectControllerTests
    {
		private readonly Mock<ITransferProjectQueryService> _mockTransferProjectQueryService;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<ILogger<TransferProjectController>> _loggerMock;
		private readonly TransferProjectController _controller;

		public TransferProjectControllerTests()
		{
			_mockTransferProjectQueryService = new Mock<ITransferProjectQueryService>();
			_mockMediator = new Mock<IMediator>();
			_loggerMock = new Mock<ILogger<TransferProjectController>>();
			_controller = new TransferProjectController(_mockMediator.Object, _mockTransferProjectQueryService.Object, _loggerMock.Object);
		}

		private SetTransferPublicEqualityDutyCommand CreateValidSetPublicEqualityDutyCommand()
		{
			return new SetTransferPublicEqualityDutyCommand(
				urn: 1001,
				publicEqualityDutyImpact: "Likely",
				publicEqualityDutyReduceImpactReason: "Likely test reason",
				publicEqualityDutySectionComplete: true
			);
		}

		[Fact]
		public async Task SetPublicEqualityDuty_ReturnsOk_WhenUpdateIsSuccessful()
		{
			var request = CreateValidSetPublicEqualityDutyCommand();

			_mockMediator.Setup(m => m.Send(It.IsAny<SetTransferPublicEqualityDutyCommand>(), default))
						 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetTransferPublicEqualityDuty(request.Urn, request);

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetPublicEqualityDuty_ReturnsNotFound_WhenProjectNotFound()
		{
			var request = CreateValidSetPublicEqualityDutyCommand();
			_mockMediator.Setup(m => m.Send(It.IsAny<SetTransferPublicEqualityDutyCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetTransferPublicEqualityDuty(request.Urn, request);

			Assert.IsType<NotFoundResult>(result);
		}
	}
}
