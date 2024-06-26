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
	public class SetProjectDatesCommandHandlerTests
	{
		private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private readonly Mock<ILogger<SetProjectDatesCommandHandler>> _mockLogger;
		private readonly IRequestHandler<SetProjectDatesCommand, CommandResult> _handler;

		public SetProjectDatesCommandHandlerTests()
		{
			_mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
						  .ReturnsAsync(1);
			_mockConversionProjectRepository.Setup(repo => repo.UnitOfWork).Returns(mockUnitOfWork.Object);

			_mockLogger = new Mock<ILogger<SetProjectDatesCommandHandler>>();
			_handler = new SetProjectDatesCommandHandler(_mockConversionProjectRepository.Object, _mockLogger.Object);
		}

		private SetProjectDatesCommand CreateValidSetProjectDatesCommand()
		{
			DateTime? date = DateTime.Now;
			return new SetProjectDatesCommand(
				1,
				date,
				date.Value.AddDays(-1),
				date.Value.AddDays(-3),
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
			var command = CreateValidSetProjectDatesCommand();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id))
											.ReturnsAsync(null as Project);

			var result = await _handler.Handle(command, CancellationToken.None);

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task Handle_ReturnsSuccess_WhenProjectExists()
		{
			var command = CreateValidSetProjectDatesCommand();
			var existingProject = CreateMockProject();
			_mockConversionProjectRepository.Setup(repo => repo.GetConversionProject(command.Id))
											.ReturnsAsync(existingProject);

			var result = await _handler.Handle(command, CancellationToken.None);

			_mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
