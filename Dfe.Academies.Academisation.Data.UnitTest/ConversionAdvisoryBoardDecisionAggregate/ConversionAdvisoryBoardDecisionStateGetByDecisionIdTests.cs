﻿using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateGetByDecisionIdTests
{
	private readonly AdvisoryBoardDecisionGetDataByDecisionIdQuery _target;

	public ConversionAdvisoryBoardDecisionStateGetByDecisionIdTests()
	{
		var mockContext = new TestAdvisoryBoardDecisionContext().CreateContext();
		_target = new(mockContext);
	}

	[Fact]
	public async Task WhenRecordExists___ShouldReturnExpectedConversionAdvisoryBoardDecision()
	{
		const int expectedDecisionId = 2;

		//Act
		var result = await _target.Execute(expectedDecisionId);

		//Assert
		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecision>(result),
			() => Assert.Equal(expectedDecisionId, result!.AdvisoryBoardDecisionDetails.ConversionProjectId),
			() => Assert.NotEqual(default, result!.Id),
			() => Assert.NotEqual(default, result!.CreatedOn),
			() => Assert.NotEqual(default, result!.LastModifiedOn)
		);
	}

	[Fact]
	public async Task WhenRecordDoesNotExist___ShouldReturnNull()
	{
		const int decisionId = 4;

		//Act
		var result = await _target.Execute(decisionId);

		//Assert
		Assert.Null(result);
	}
}
