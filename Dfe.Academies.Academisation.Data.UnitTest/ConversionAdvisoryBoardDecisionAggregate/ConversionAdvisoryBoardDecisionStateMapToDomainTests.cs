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
			state.Id,
			state.ConversionProjectId,
			state.Decision,
			state.ApprovedConditionsSet,
			state.ApprovedConditionsDetails,
			state.DeclinedReasons!.Select(reason => reason.Reason).ToList(),
			state.DeclinedOtherReason,
			state.DeferredReasons!.Select(reason => reason.Reason).ToList(),
			state.DeferredOtherReason,
			state.AdvisoryBoardDecisionDate,
			state.DecisionMadeBy
		);

		ConversionAdvisoryBoardDecision expected = new(state.Id, details, state.CreatedOn, state.LastModifiedOn);
	
		//Act
		var result = state.MapToDomain();
		
		//Assert
		Assert.Equivalent(expected, result);
	}
}
