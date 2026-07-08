using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
    public class SetSfsoCommissioningCommandHandlerTests
    {
        private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository;
        private readonly Mock<ILogger<SetSfsoCommissioningCommandHandler>> _mockLogger;
        private readonly SetSfsoCommissioningCommandHandler _handler;

        public SetSfsoCommissioningCommandHandlerTests()
        {
            _mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockConversionProjectRepository.Setup(repo => repo.UnitOfWork).Returns(mockUnitOfWork.Object);

            _mockLogger = new Mock<ILogger<SetSfsoCommissioningCommandHandler>>();
            _handler = new SetSfsoCommissioningCommandHandler(_mockConversionProjectRepository.Object, _mockLogger.Object);
        }

        private static Project CreateMockProject() => new Project(1, new ProjectDetails());

        [Fact]
        public async Task Handle_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            var command = new SetSfsoCommissioningCommand(1, "overview", true);
            _mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id, default))
                                            .ReturnsAsync(null as Project);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<NotFoundCommandResult>(result);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessAndStores_WhenProjectExists()
        {
            var command = new SetSfsoCommissioningCommand(1, "overview text", true);
            var existingProject = CreateMockProject();
            _mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id, default))
                                            .ReturnsAsync(existingProject);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<CommandSuccessResult>(result);
            _mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
            Assert.Equal("overview text", existingProject.Details.SfsoCommissioningOverview);
            Assert.True(existingProject.Details.SfsoCommissioningSectionComplete);
        }

        [Fact]
        public async Task Handle_ReturnsValidationError_WhenOverviewExceeds250Characters()
        {
            var command = new SetSfsoCommissioningCommand(1, new string('x', 251), false);

            var result = await _handler.Handle(command, CancellationToken.None);

            var validationResult = Assert.IsType<CommandValidationErrorResult>(result);
            Assert.Contains(validationResult.ValidationErrors, e => e.PropertyName == "SfsoCommissioningOverview");
            _mockConversionProjectRepository.Verify(
                repo => repo.GetConversionProject(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenOverviewIsExactly250Characters()
        {
            var command = new SetSfsoCommissioningCommand(1, new string('x', 250), true);
            var existingProject = CreateMockProject();
            _mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id, default))
                                            .ReturnsAsync(existingProject);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<CommandSuccessResult>(result);
        }
    }
}