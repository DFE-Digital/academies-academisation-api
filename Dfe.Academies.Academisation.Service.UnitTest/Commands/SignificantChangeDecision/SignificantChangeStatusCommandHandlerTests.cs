using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChangeDecision;

public class SignificantChangeStatusCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<ILogger<SignificantChangeStatusCommandHandler>> _loggerMock = new();

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SignificantChangeStatusCommand(100, Decision.Approved, null);
		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await CreateHandler().Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Theory]
	[InlineData(Decision.Approved, false, SignificantChangeStatus.Approved)]
	[InlineData(Decision.Approved, true, SignificantChangeStatus.ApprovedWithConditions)]
	[InlineData(Decision.Declined, false, SignificantChangeStatus.Declined)]
	[InlineData(Decision.Deferred, false, SignificantChangeStatus.Deferred)]
	[InlineData(Decision.Withdrawn, false, SignificantChangeStatus.Withdrawn)]
	public async Task Handle_ProjectFound_UpdatesStatusAndPersistsChanges(
		Decision decision,
		bool approvedWithConditions,
		SignificantChangeStatus expectedStatus)
	{
		var command = new SignificantChangeStatusCommand(100, decision, approvedWithConditions);
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change", "Test School"),
			DateTime.UtcNow);
		_repositoryMock.Setup(x => x.UnitOfWork).Returns(_unitOfWorkMock.Object);
		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await CreateHandler().Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Status.Should().Be(expectedStatus);
		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private SignificantChangeStatusCommandHandler CreateHandler() =>
		new(_repositoryMock.Object, _loggerMock.Object);
}