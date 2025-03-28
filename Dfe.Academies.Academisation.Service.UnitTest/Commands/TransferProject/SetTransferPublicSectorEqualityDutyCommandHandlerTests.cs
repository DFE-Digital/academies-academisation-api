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

		/*
		 
		
		
	[Fact]
	public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
	{
		// Arrange
		var command = new SetTransferProjectGeneralInformationCommand
		{
			Urn = 1,
			Recommendation = "Recommendation",
			Author = "Author"
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
		var command = new SetTransferProjectGeneralInformationCommand
		{
			Urn = 1,
			Recommendation = "Recommendation",
			Author = "Author"
		};
		// Create a transfer project to 'SetTrustInformationAndProjectDates' to
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

		transferProject.Recommendation.Should().Be(command.Recommendation);
		transferProject.Author.Should().Be(command.Author);

		_transferProjectRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
	}
		  
		 
		 */

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			var loggerMock = new Mock<ILogger<SetTransferPublicSectorEqualityDutyCommandHandler>>();

			transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>()))
				.ReturnsAsync((Domain.TransferProjectAggregate.TransferProject)null);

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
