using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class TransferProjectControllerTests : CommandResult 
	{
		private readonly Mock<ITransferProjectQueryService> _mockQuery = new();
		private readonly Mock<IMediator> _mockMediator = new();
		private readonly Mock<ILogger<TransferProjectController>> _mockLogger = new();

		[Fact]
		public async Task SetTransferPublicSectorEqualityDuty()
		{
			//Arrange
			_mockMediator
				.Setup(c => c.Send(It.IsAny<SetTransferPublicSectorEqualityDutyCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandSuccessResult());

			var subject = new TransferProjectController(
				_mockMediator.Object,
				_mockQuery.Object,
				_mockLogger.Object);

			var command = new SetTransferPublicSectorEqualityDutyCommand
			{
				Urn = 12345,
				HowLikelyImpactProtectedCharacteristics = Likelyhood.Likely,
				WhatWillBeDoneToReduceImpact = "Test",
				IsCompleted = true
			};

			//Act
			var result = await subject.SetTransferPublicSectorEqualityDuty(12345 ,command, It.IsAny<CancellationToken>());

			//Assert
			Assert.IsType<OkResult>(result);
		}
	}
}
