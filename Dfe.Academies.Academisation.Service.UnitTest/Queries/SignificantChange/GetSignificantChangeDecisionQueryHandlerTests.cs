using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
using AutoMapper;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries.SignificantChange;

public class GetSignificantChangeDecisionQueryHandlerTests
{
	private readonly Mock<IAdvisoryBoardDecisionRepository> _repositoryMock = new();

	[Fact]
	public async Task Handle_WhenDecisionExists_ReturnsMappedServiceModel()
	{
		var projectId = 101;
		var now = DateTime.UtcNow;
		var details = new AdvisoryBoardDecisionDetails(
			null,
			null,
			projectId,
			AdvisoryBoardDecision.Approved,
			true,
			"Conditions",
			now,
			null,
			DecisionMadeBy.DirectorGeneral,
			"Decision Maker");

		var decision = new ConversionAdvisoryBoardDecision(
			55,
			details,
			new List<AdvisoryBoardDeferredReasonDetails>(),
			new List<AdvisoryBoardDeclinedReasonDetails>(),
			new List<AdvisoryBoardWithdrawnReasonDetails>(),
			new List<AdvisoryBoardDAORevokedReasonDetails>(),
			now,
			now);

		_repositoryMock
			.Setup(r => r.GetSignificantChangeDecision(projectId))
			.ReturnsAsync(decision);

		var target = new GetSignificantChangeDecisionQueryHandler(_repositoryMock.Object);

		var result = await target.Handle(new GetSignificantChangeDecisionQuery(projectId), default);

		result.Should().NotBeNull();
		result!.AdvisoryBoardDecisionId.Should().Be(55);
		result.SignificantChangeProjectId.Should().Be(projectId);
		result.Decision.Should().Be(Decision.Approved);
		_repositoryMock.Verify(r => r.GetSignificantChangeDecision(projectId), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenDecisionDoesNotExist_ReturnsNull()
	{
		const int projectId = 999;

		_repositoryMock
			.Setup(r => r.GetSignificantChangeDecision(projectId))
			.ReturnsAsync((ConversionAdvisoryBoardDecision?)null);

		var target = new GetSignificantChangeDecisionQueryHandler(_repositoryMock.Object);

		var result = await target.Handle(new GetSignificantChangeDecisionQuery(projectId), default);

		result.Should().BeNull();
		_repositoryMock.Verify(r => r.GetSignificantChangeDecision(projectId), Times.Once);
	}
}
