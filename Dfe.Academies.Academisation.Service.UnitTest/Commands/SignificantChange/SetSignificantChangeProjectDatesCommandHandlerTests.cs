using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeProjectDatesCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeProjectDatesCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeProjectDatesCommandHandler _handler;

	public SetSignificantChangeProjectDatesCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeProjectDatesCommandHandler>>();
		_handler = new SetSignificantChangeProjectDatesCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeProjectDatesCommand(
			100,
			DateTime.UtcNow,
			DateTime.UtcNow);

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Theory]
	[InlineData("2024-07-01", "2024-07-15")]
	[InlineData(null, "2024-08-15")]
	[InlineData("2024-09-01", null)]
	[InlineData(null, null)]
	public async Task Handle_ProjectFound_UpdatesSectionAndPersistsChanges(string? proposedDecisionDateString, string? proposedChangeDateString)
	{
		DateTime? proposedDecisionDate = string.IsNullOrEmpty(proposedDecisionDateString) ? null : DateTime.Parse(proposedDecisionDateString);
		DateTime? proposedChangeDate = string.IsNullOrEmpty(proposedChangeDateString) ? null : DateTime.Parse(proposedChangeDateString);

		var command = new SetSignificantChangeProjectDatesCommand(
			100,
			proposedDecisionDate,
			proposedChangeDate);

		var project = SignificantChangeProject.Create(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);

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

		project.Details.ProposedChangeDate.Should().Be(proposedChangeDate);
		project.Details.ProposedDecisionDate.Should().Be(proposedDecisionDate);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
