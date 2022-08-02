using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Xunit;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest;

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
		//Arrange
		var state = _fixture.Create<ConversionAdvisoryBoardDecisionState>();
		AdvisoryBoardDecisionDetails details = new(
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

		ConversionAdvisoryBoardDecision expected = new(state.Id, details);
		
		//Act
		var result = state.MapToDomain();
		
		//Assert
		Assert.Equivalent(expected, result);
	}
}
