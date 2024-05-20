using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferringAcademyGeneralInformationCommandHandlerTests
	{
		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			var loggerMock = new Mock<ILogger<SetTransferringAcademyGeneralInformationCommandHandler>>();

			transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>()))
				.ReturnsAsync((Domain.TransferProjectAggregate.TransferProject)null);

			var handler = new SetTransferringAcademyGeneralInformationCommandHandler(transferProjectRepositoryMock.Object, loggerMock.Object);
			var command = new SetTransferringAcademyGeneralInformationCommand
			{
				Urn = 1,
				TransferringAcademyUkprn = "12345678",
				PFIScheme = "PFI Scheme"
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

			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "Outgoing Trust", null, null, new List<string> { "12345678" }, false, DateTime.Now);

			var transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var loggerMock = new Mock<ILogger<SetTransferringAcademyGeneralInformationCommandHandler>>();

			transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>()))
										 .ReturnsAsync(transferProject);
			transferProjectRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);

			var handler = new SetTransferringAcademyGeneralInformationCommandHandler(transferProjectRepositoryMock.Object, loggerMock.Object);
			var command = new SetTransferringAcademyGeneralInformationCommand
			{
				Urn = 1,
				TransferringAcademyUkprn = "12345678",
				PFIScheme = "PFI Scheme"
			};

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			transferProjectRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
			unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			Assert.IsType<CommandSuccessResult>(result);
			Assert.Equal("PFI Scheme", transferProject.TransferringAcademies.FirstOrDefault().PFIScheme);
		}
	}
}
