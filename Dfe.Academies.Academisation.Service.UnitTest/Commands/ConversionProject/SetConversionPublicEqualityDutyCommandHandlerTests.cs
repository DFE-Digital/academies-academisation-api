using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
    public class SetConversionPublicEqualityDutyCommandHandlerTests
    {
		private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private readonly Mock<ILogger<SetConversionPublicEqualityDutyCommandHandler>> _mockLogger;
		private readonly IRequestHandler<SetConversionPublicEqualityDutyCommand, CommandResult> _handler;

		public SetConversionPublicEqualityDutyCommandHandlerTests()
		{
			_mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
						  .ReturnsAsync(1);
			_mockConversionProjectRepository.Setup(repo => repo.UnitOfWork).Returns(mockUnitOfWork.Object);

			_mockLogger = new Mock<ILogger<SetConversionPublicEqualityDutyCommandHandler>>();
			_handler = new SetConversionPublicEqualityDutyCommandHandler(_mockConversionProjectRepository.Object, _mockLogger.Object);
		}

		private SetConversionPublicEqualityDutyCommand CreateValidSetConversionPublicEqualityDutyCommand()
		{
			return new SetConversionPublicEqualityDutyCommand(
				id: 1,
				publicEqualityDutyImpact: "Likely",
				publicEqualityDutyReduceImpactReason: "Likely reason",
				publicEqualityDutySectionComplete: true
			);
		}

		private Project CreateMockProject()
		{
			var projectDetails = new ProjectDetails();
			return new Project(1, projectDetails);
		}

		[Fact]
		public async Task Handle_ReturnsNotFound_WhenProjectDoesNotExist()
		{
			var command = CreateValidSetConversionPublicEqualityDutyCommand();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id, default))
											.ReturnsAsync(null as Project);

			var result = await _handler.Handle(command, CancellationToken.None);

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task Handle_ReturnsSuccess_WhenProjectExists()
		{
			var command = CreateValidSetConversionPublicEqualityDutyCommand();
			var existingProject = CreateMockProject();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id, default))
											.ReturnsAsync(existingProject);

			var result = await _handler.Handle(command, CancellationToken.None);

			_mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
