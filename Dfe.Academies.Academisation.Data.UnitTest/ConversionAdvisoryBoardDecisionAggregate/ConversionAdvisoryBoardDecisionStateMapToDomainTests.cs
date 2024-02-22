using System;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateMapToDomainTests
{
	private readonly Fixture _fixture = new();

	[Fact]
	public void ShouldReturnConversionAdvisoryBoardDecision()
	{
		//Arrange
		var state = _fixture.Create<AdvisoryBoardDecisionState>();

		//Act
		var result = state.MapToDomain();

		//Assert
		Assert.Multiple(
			() => Assert.IsAssignableFrom<IConversionAdvisoryBoardDecision>(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecision>(result)
		);
	}

	[Fact]
	public void ShouldReturnExpectedConversionAdvisoryBoardDecisionState()
	{
		var timestamp = DateTime.UtcNow;
		//Arrange
		var state = _fixture.Build<AdvisoryBoardDecisionState>()
			.With(s => s.CreatedOn, timestamp)
			.With(s => s.LastModifiedOn, timestamp)
			.With(s => s.TransferProjectId, (int?)null)
			.Create();

		AdvisoryBoardDecisionDetails details = new(
			state.ConversionProjectId,
			state.TransferProjectId,
			state.Decision,
			state.ApprovedConditionsSet,
			state.ApprovedConditionsDetails,
			state.DeclinedReasons!
				.Select(reason => new AdvisoryBoardDeclinedReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			state.DeferredReasons!
				.Select(reason => new AdvisoryBoardDeferredReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			state.WithdrawnReasons!
				.Select(reason => new AdvisoryBoardWithdrawnReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			state.AdvisoryBoardDecisionDate,
			state.AcademyOrderDate,
			state.DecisionMadeBy
		);

		//Act
		var result = state.MapToDomain();

		//Assert
		Assert.Multiple(
			() => Assert.Equivalent(details, result.AdvisoryBoardDecisionDetails),
			() => Assert.Equal(timestamp, result.CreatedOn),
			() => Assert.Equal(timestamp, result.LastModifiedOn),
			() => Assert.NotEqual(default, result.Id)
		);
	}
}
