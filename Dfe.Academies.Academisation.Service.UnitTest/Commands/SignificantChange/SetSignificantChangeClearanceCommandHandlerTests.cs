using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeClearanceCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeClearanceCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeClearanceCommandHandler _handler;

	public SetSignificantChangeClearanceCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeClearanceCommandHandler>>();
		_handler = new SetSignificantChangeClearanceCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeClearanceCommand(
			Id: 100,
			RegionsGroupClearance: "Jane Smith",
			TrustRegulationClearance: "John Doe");

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task Handle_ProjectFound_UpdatesClearanceAndPersistsChanges()
	{
		var command = new SetSignificantChangeClearanceCommand(
			Id: 200,
			RegionsGroupClearance: "Jane Smith",
			TrustRegulationClearance: "John Doe");

		var project = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
				123456,
				1,
				"Test Trust",
				"12345678",
				"Change of age range",
				"Test School"), DateTime.UtcNow
		);

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock
			.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		_repositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.RegionsGroupClearance.Should().Be("Jane Smith");
		project.Details.TrustRegulationClearance.Should().Be("John Doe");
		project.Details.GetClearanceTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
