using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferPublicEqualityDutyCommandHandlerTests
	{
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<ILogger<SetTransferPublicEqualityDutyCommandHandler>> _loggerMock;
		private readonly SetTransferPublicEqualityDutyCommandHandler _handler;

		public SetTransferPublicEqualityDutyCommandHandlerTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_loggerMock = new Mock<ILogger<SetTransferPublicEqualityDutyCommandHandler>>();
			_handler = new SetTransferPublicEqualityDutyCommandHandler(_transferProjectRepositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = new SetTransferPublicEqualityDutyCommand(
				urn: 1,
				publicEqualityDutyImpact: "Likely",
				publicEqualityDutyReduceImpactReason: "Likely reason",
				publicEqualityDutySectionComplete: true
			);

			_transferProjectRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))!
				.ReturnsAsync((Domain.TransferProjectAggregate.TransferProject)null);

			// Act
			var result = await _handler.Handle(command, default);

			// Assert
			result.Should().BeOfType<NotFoundCommandResult>();
			_loggerMock.Verify(logger =>
					logger.Log(
						LogLevel.Error,
						It.IsAny<EventId>(),
						It.Is<It.IsAnyType>((v, t) => true),
						It.IsAny<Exception>(),
						It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
				Times.Once);
		}
	}
}
