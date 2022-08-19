using System;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
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
		var state = _fixture.Create<ConversionAdvisoryBoardDecisionState>();

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
		var state = _fixture.Build<ConversionAdvisoryBoardDecisionState>()
			.With(s => s.CreatedOn, timestamp)
			.With(s => s.LastModifiedOn, timestamp)
			.Create();

		AdvisoryBoardDecisionDetails details = new(
			state.ConversionProjectId,
			state.Decision,
			state.ApprovedConditionsSet,
			state.ApprovedConditionsDetails,
			state.DeclinedReasons!
				.Select(reason => new AdvisoryBoardDeclinedReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			state.DeferredReasons!
				.Select(reason => new AdvisoryBoardDeferredReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			state.AdvisoryBoardDecisionDate,
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
