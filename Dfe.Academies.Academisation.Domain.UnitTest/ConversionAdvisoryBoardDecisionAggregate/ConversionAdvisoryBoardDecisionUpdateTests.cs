using System;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionUpdateTests
{
	[Fact]
	public void ValidatorReturnsSuccessResult__ReturnsCommandSuccessResult_DecisionMutated()
	{
		//Arrange 
		var timestamp = DateTime.UtcNow;

		AdvisoryBoardDecisionDetails details = new(
			1,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			timestamp.AddDays(-1),
			DecisionMadeBy.DirectorGeneral);

		var updatedDetails = details with { DecisionMadeBy = DecisionMadeBy.Minister };

		ConversionAdvisoryBoardDecision expected = new(1, updatedDetails, timestamp, timestamp);
		ConversionAdvisoryBoardDecision target = new(1, details, timestamp, timestamp);

		//Act
		var result = target.Update(updatedDetails);

		//Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => Assert.Equivalent(expected, target)
		);
	}

	[Fact]
	public void ValidatorReturnsValidationErrorResult___ReturnsCommandValidationErrorResult_DecisionNotMutated()
	{
		//Arrange 
		var timestamp = DateTime.UtcNow;

		AdvisoryBoardDecisionDetails details = new(
			1,
			null,
			AdvisoryBoardDecision.Approved,
			null,
			null,
			null,
			null,
			timestamp.AddDays(-1),
			DecisionMadeBy.DirectorGeneral);

		var updatedDetails = details with { DecisionMadeBy = DecisionMadeBy.Minister };

		ConversionAdvisoryBoardDecision expected = new(1, details, timestamp, timestamp);
		ConversionAdvisoryBoardDecision target = new(1, details, timestamp, timestamp);

		//Act
		var result = target.Update(updatedDetails);

		//Assert
		Assert.Multiple(
			() => Assert.IsType<CommandValidationErrorResult>(result),
			() => Assert.Equivalent(expected, target)
		);
	}
}
