﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommandHandlerTests
	{
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<ILogger<SetTransferProjectFeaturesCommandHandler>> _loggerMock;
		private readonly SetTransferProjectFeaturesCommandHandler _handler;

		public SetTransferProjectFeaturesCommandHandlerTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_loggerMock = new Mock<ILogger<SetTransferProjectFeaturesCommandHandler>>();
			_handler = new SetTransferProjectFeaturesCommandHandler(_transferProjectRepositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = new SetTransferProjectFeaturesCommand
			{
				Urn = 1,
				TypeOfTransfer = "TestType",
				WhoInitiatedTheTransfer = "TestInitiator",
				IsCompleted = true
			};

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

		[Fact]
		public async Task Handle_TransferProjectFound_ReturnsCommandSuccessResult()
		{
			// Arrange
			var command = new SetTransferProjectFeaturesCommand
			{
				Urn = 1,
				TypeOfTransfer = "TestType",
				WhoInitiatedTheTransfer = "TestInitiator",
				IsCompleted = true
			};
			// Create a transfer project to 'SetFeatures' to
			var transferringAcademies = new List<TransferringAcademy>() { new TransferringAcademy("23456789", "in trust", "34567890", "", "") };
			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", transferringAcademies, false, DateTime.Now);
			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Mock GetById to use our Transfer Project from above
			_transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>())).ReturnsAsync(transferProject);


			// Act
			var result = await _handler.Handle(command, new CancellationToken(false));

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			transferProject.TypeOfTransfer.Should().Be(command.TypeOfTransfer);
			transferProject.WhoInitiatedTheTransfer.Should().Be(command.WhoInitiatedTheTransfer);
			transferProject.FeatureSectionIsCompleted.Should().Be(command.IsCompleted);

			_transferProjectRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
		}

	}
}
