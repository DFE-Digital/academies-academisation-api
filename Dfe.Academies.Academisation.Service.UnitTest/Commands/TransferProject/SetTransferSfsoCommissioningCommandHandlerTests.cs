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
	public class SetTransferSfsoCommissioningCommandHandlerTests
	{
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<ILogger<SetTransferSfsoCommissioningCommandHandler>> _loggerMock;
		private readonly SetTransferSfsoCommissioningCommandHandler _handler;

		public SetTransferSfsoCommissioningCommandHandlerTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_loggerMock = new Mock<ILogger<SetTransferSfsoCommissioningCommandHandler>>();
			_handler = new SetTransferSfsoCommissioningCommandHandler(_transferProjectRepositoryMock.Object, _loggerMock.Object);

			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
			_transferProjectRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(unitOfWorkMock.Object);
		}

		private static Domain.TransferProjectAggregate.TransferProject CreateProject()
		{
			var transferringAcademies = new List<TransferringAcademy>
			{
				new TransferringAcademy("23456789", "in trust", "34567890", "", "")
			};
			return Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", transferringAcademies, false, DateTime.Now);
		}

		[Fact]
		public async Task Handle_ReturnsNotFound_WhenProjectDoesNotExist()
		{
			var command = new SetTransferSfsoCommissioningCommand(1, "overview");
			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>()))
				.ReturnsAsync((Domain.TransferProjectAggregate.TransferProject)null);

			var result = await _handler.Handle(command, default);

			result.Should().BeOfType<NotFoundCommandResult>();
		}

		[Fact]
		public async Task Handle_ReturnsSuccessAndStores_WhenProjectExists()
		{
			var project = CreateProject();
			var command = new SetTransferSfsoCommissioningCommand(1, "overview text");
			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>())).ReturnsAsync(project);

			var result = await _handler.Handle(command, default);

			result.Should().BeOfType<CommandSuccessResult>();
			project.SfsoCommissioningOverview.Should().Be("overview text");
			_transferProjectRepositoryMock.Verify(r => r.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
		}

		[Fact]
		public async Task Handle_ReturnsValidationError_WhenOverviewExceeds250Characters()
		{
			var command = new SetTransferSfsoCommissioningCommand(1, new string('x', 251));

			var result = await _handler.Handle(command, default);

			result.Should().BeOfType<CommandValidationErrorResult>()
				.Which.ValidationErrors.Should().Contain(e => e.PropertyName == "SfsoCommissioningOverview");
			_transferProjectRepositoryMock.Verify(r => r.GetByUrn(It.IsAny<int>()), Times.Never);
		}

		[Fact]
		public async Task Handle_ReturnsSuccess_WhenOverviewIsExactly250Characters()
		{
			var project = CreateProject();
			var command = new SetTransferSfsoCommissioningCommand(1, new string('x', 250));
			_transferProjectRepositoryMock.Setup(r => r.GetByUrn(It.IsAny<int>())).ReturnsAsync(project);

			var result = await _handler.Handle(command, default);

			result.Should().BeOfType<CommandSuccessResult>();
		}
	}
}