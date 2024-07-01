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
	public class SetTransferProjectTransferDatesCommandHandlerTests
	{
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<ILogger<SetTransferProjectTransferDatesCommandHandler>> _loggerMock;
		private readonly SetTransferProjectTransferDatesCommandHandler _handler;

		public SetTransferProjectTransferDatesCommandHandlerTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_loggerMock = new Mock<ILogger<SetTransferProjectTransferDatesCommandHandler>>();
			_handler = new SetTransferProjectTransferDatesCommandHandler(_transferProjectRepositoryMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = 1,
				HtbDate = DateTime.UtcNow,
				TargetDateForTransfer = DateTime.UtcNow.AddMonths(1)
			};

			_transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>()))
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
			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = 1,
				HtbDate = DateTime.UtcNow,
				TargetDateForTransfer = DateTime.UtcNow.AddMonths(1)
			};

			// Create a transfer project to 'SetTransferDates' to
			var transferringAcademies = new List<TransferringAcademy>
			{
				new TransferringAcademy("23456789", "in trust", "34567890", "", "")
			};
			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", transferringAcademies, false, DateTime.Now);

			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Mock GetByUrn to use our Transfer Project from above
			_transferProjectRepositoryMock.Setup(x => x.GetByUrn(It.IsAny<int>())).ReturnsAsync(transferProject);

			// Act
			var result = await _handler.Handle(command, new CancellationToken(false));

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			transferProject.HtbDate.Should().Be(command.HtbDate);
			transferProject.TargetDateForTransfer.Should().Be(command.TargetDateForTransfer);

			_transferProjectRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
		}

		[Fact]
		public async Task Handle_ShouldFireEvent_WhenTargetDateChanges()
		{
			// Arrange
			var transferringAcademies = new List<TransferringAcademy>
			{
				new TransferringAcademy("23456789", "in trust", "34567890", "", "")
			};

			var project = Domain.TransferProjectAggregate.TransferProject.Create("12345", "Outgoing Trust", transferringAcademies, false, DateTime.UtcNow);
			project.SetTransferDates(DateTime.UtcNow, DateTime.UtcNow, DateTime.Parse("2023-01-01"), false);

			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>())).ReturnsAsync(project);

			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = 1,
				TargetDateForTransfer = DateTime.Parse("2023-12-31"),
				ChangedBy = "TestUser"
			};

			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();
			Assert.Single(project.DomainEvents);
			Assert.IsType<OpeningDateChangedDomainEvent>(project.DomainEvents.First());
		}

		[Fact]
		public async Task Handle_ShouldNotFireEvent_WhenOldDateIsNull()
		{
			// Arrange
			var transferringAcademies = new List<TransferringAcademy>
			{
				new TransferringAcademy("23456789", "in trust", "34567890", "", "")
			};

			var project = Domain.TransferProjectAggregate.TransferProject.Create("12345", "Outgoing Trust", transferringAcademies, false, DateTime.UtcNow);
			project.SetTransferDates(DateTime.UtcNow, DateTime.UtcNow, null, false);

			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>())).ReturnsAsync(project);

			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = 1,
				TargetDateForTransfer = DateTime.Parse("2023-12-31"),
				ChangedBy = "TestUser"
			};

			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();
			Assert.Null(project.DomainEvents);
		}

		[Fact]
		public async Task Handle_ShouldNotFireEvent_WhenDatesDoNotChange()
		{
			// Arrange
			var transferringAcademies = new List<TransferringAcademy>
			{
				new TransferringAcademy("23456789", "in trust", "34567890", "", "")
			};

			var project = Domain.TransferProjectAggregate.TransferProject.Create("12345", "Outgoing Trust", transferringAcademies, false, DateTime.UtcNow);
			project.SetTransferDates(DateTime.UtcNow, DateTime.UtcNow, DateTime.Parse("2023-12-31"), false);

			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>())).ReturnsAsync(project);

			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = 1,
				TargetDateForTransfer = DateTime.Parse("2023-12-31"),
				ChangedBy = "TestUser"
			};

			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();
			Assert.Null(project.DomainEvents);
		}
	}
}
