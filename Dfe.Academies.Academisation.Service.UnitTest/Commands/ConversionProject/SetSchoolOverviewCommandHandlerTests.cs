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
	public class SetSchoolOverviewCommandHandlerTests
	{
		private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private readonly Mock<ILogger<SetSchoolOverviewCommandHandler>> _mockLogger;
		private readonly IRequestHandler<SetSchoolOverviewCommand, CommandResult> _handler;

		public SetSchoolOverviewCommandHandlerTests()
		{
			_mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
						  .ReturnsAsync(1);
			_mockConversionProjectRepository.Setup(repo => repo.UnitOfWork).Returns(mockUnitOfWork.Object);

			_mockLogger = new Mock<ILogger<SetSchoolOverviewCommandHandler>>();
			_handler = new SetSchoolOverviewCommandHandler(_mockConversionProjectRepository.Object, _mockLogger.Object);
		}

		private SetSchoolOverviewCommand CreateValidSetSchoolOverviewCommand()
		{
			return new SetSchoolOverviewCommand(
				1,
				"100",
				"No issues",
				"No",
				"None",
				150m,
				10m,
				5m,
				"N/A",
				20m,
				"Within city limits",
				"Jane Doe - PartyName",
				true,
				true,
				true
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
			var command = CreateValidSetSchoolOverviewCommand();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id))
											.ReturnsAsync(null as Project);

			var result = await _handler.Handle(command, CancellationToken.None);

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task Handle_ReturnsSuccess_WhenProjectExists()
		{
			var command = CreateValidSetSchoolOverviewCommand();
			var existingProject = CreateMockProject();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id))
											.ReturnsAsync(existingProject);

			var result = await _handler.Handle(command, CancellationToken.None);

			_mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
