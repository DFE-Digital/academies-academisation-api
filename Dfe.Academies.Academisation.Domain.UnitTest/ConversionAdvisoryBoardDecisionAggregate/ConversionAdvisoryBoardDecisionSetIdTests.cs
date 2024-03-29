﻿using System;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionSetIdTests
{
	private readonly Fixture _fixture = new();

	[Fact]
	public void IdHasAlreadyBeenSet___ThrowsException()
	{
		//Arrange
		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		ConversionAdvisoryBoardDecision target = new(1, details, default, default);

		//Act & Assert
		Assert.Throws<InvalidOperationException>(() => target.SetId(1));
	}

	[Fact]
	public void IdHasNotBeenSet__MutatesDecision()
	{
		//Arrange
		const int expectedId = 4;

		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		ConversionAdvisoryBoardDecision expected = new(expectedId, details, default, default);
		ConversionAdvisoryBoardDecision target = new(default, details, default, default);

		//Act
		target.SetId(expectedId);

		//Assert
		Assert.Equivalent(expected, target);
	}
}
