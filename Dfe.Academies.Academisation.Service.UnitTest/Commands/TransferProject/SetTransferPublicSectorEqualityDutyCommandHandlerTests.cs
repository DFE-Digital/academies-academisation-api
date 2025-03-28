using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferPublicSectorEqualityDutyCommandHandlerTests
	{

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			var loggerMock = new Mock<ILogger<SetTransferPublicSectorEqualityDutyCommandHandler>>();

			var handler = new SetTransferPublicSectorEqualityDutyCommandHandler(transferProjectRepositoryMock.Object, loggerMock.Object);
			var command = new SetTransferPublicSectorEqualityDutyCommand
			{
				Urn = 1,
				IsCompleted = true
			};

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task Handle_ValidCommand_ReturnsCommandSuccessResult()
		{
			// Arrange
			var transferringAcademies = new List<TransferringAcademy>() { new TransferringAcademy("23456789", "in trust", "34567890", "", "") };
			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "Outgoing Trust", transferringAcademies, false, DateTime.Now);
			// Create a transfer project to 'SetTrustInformationAndProjectDates' to

			var transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var loggerMock = new Mock<ILogger<SetTransferPublicSectorEqualityDutyCommandHandler>>();

			transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>()))
										 .ReturnsAsync(transferProject);
			transferProjectRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);

			var handler = new SetTransferPublicSectorEqualityDutyCommandHandler(transferProjectRepositoryMock.Object, loggerMock.Object);
			var command = new SetTransferPublicSectorEqualityDutyCommand
			{
				Urn = 12345678,
				HowLikelyImpactProtectedCharacteristics = Likelyhood.Likely,
				WhatWillBeDoneToReduceImpact = "Test",
				IsCompleted = true
			};

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			transferProjectRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
			unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsType<CommandSuccessResult>(result);
		}
		
	}
}
