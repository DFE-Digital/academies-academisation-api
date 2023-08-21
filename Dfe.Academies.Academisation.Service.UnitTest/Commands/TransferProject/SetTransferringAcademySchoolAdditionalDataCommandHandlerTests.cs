using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferringAcademySchoolAdditionalDataCommandHandlerTests
	{
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<ILogger<SetTransferringAcademySchoolAdditionalDataCommandHandler>> _loggerMock;
		private readonly SetTransferringAcademySchoolAdditionalDataCommandHandler _handler;

		public SetTransferringAcademySchoolAdditionalDataCommandHandlerTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_loggerMock = new Mock<ILogger<SetTransferringAcademySchoolAdditionalDataCommandHandler>>();
			_handler = new SetTransferringAcademySchoolAdditionalDataCommandHandler(_transferProjectRepositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Handle_TransferringAcademyNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = new SetTransferringAcademySchoolAdditionalDataCommand
			{
				Urn = 1,
				TransferringAcademyId = 0,
				LatestOfstedReportAdditionalInformation = "Ofsted",
				PupilNumbersAdditionalInformation = "Pupil",
				KeyStage2PerformanceAdditionalInformation= "KS2",
				KeyStage4PerformanceAdditionalInformation = "KS4",
				KeyStage5PerformanceAdditionalInformation = "KS5"
				
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
		public async Task Handle_TransferringAcademyFound_ReturnsCommandSuccessResult()
		{
			// Arrange
			var command = new SetTransferringAcademySchoolAdditionalDataCommand
			{
				Urn = 1,
				TransferringAcademyId = 0,
				LatestOfstedReportAdditionalInformation = "Ofsted",
				PupilNumbersAdditionalInformation = "Pupil",
				KeyStage2PerformanceAdditionalInformation = "KS2",
				KeyStage4PerformanceAdditionalInformation = "KS4",
				KeyStage5PerformanceAdditionalInformation = "KS5"

			};

			// Create a transfer project to 'SetSchoolAdditionalData' to
			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "23456789", new List<string> { "34567890" }, DateTime.Now);
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

			var updatedAcademy = transferProject.TransferringAcademies.FirstOrDefault(academy => academy.Id == command.TransferringAcademyId);

			updatedAcademy.Should().NotBeNull("The academy with the given ID should exist.");

			updatedAcademy.LatestOfstedReportAdditionalInformation.Should().Be("Ofsted", "The Ofsted report information should match.");
			updatedAcademy.PupilNumbersAdditionalInformation.Should().Be("Pupil", "The pupil numbers information should match.");
			updatedAcademy.KeyStage2PerformanceAdditionalInformation.Should().Be("KS2", "The Key Stage 2 performance information should match.");
			updatedAcademy.KeyStage4PerformanceAdditionalInformation.Should().Be("KS4", "The Key Stage 4 performance information should match.");
			updatedAcademy.KeyStage5PerformanceAdditionalInformation.Should().Be("KS5", "The Key Stage 5 performance information should match.");


			_transferProjectRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
		}

	}
}
