using System;
using System.Collections.Generic;
using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionFactoryTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();

	private const int ConversionProjectId = 1;

	private readonly ConversionAdvisoryBoardDecisionFactory _target = new();

	[Fact]
	public void DecisionIsInvalid___ReturnsValidationErrorResult()
	{
		//Arrange
		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		var deferred = _fixture.CreateMany<AdvisoryBoardDeferredReasonDetails>();
		var declined = _fixture.CreateMany<AdvisoryBoardDeclinedReasonDetails>();
		var withdrawn = _fixture.CreateMany<AdvisoryBoardWithdrawnReasonDetails>();

		//Act
		var result = _target.Create(details, deferred, declined, withdrawn);

		//Assert
		Assert.IsType<CreateValidationErrorResult>(result);
	}

	[Fact]
	public void DecisionIsValid___ReturnsCreateSuccessResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			true,
			_faker.Lorem.Sentence(),
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		var deferred = new List<AdvisoryBoardDeferredReasonDetails>();
		var declined = new List<AdvisoryBoardDeclinedReasonDetails>();
		var withdrawn = new List<AdvisoryBoardWithdrawnReasonDetails>();

		//Act
		var result = _target.Create(details, deferred, declined, withdrawn);

		//Assert
		var decision = Assert.IsType<CreateSuccessResult<IConversionAdvisoryBoardDecision>>(result);
		Assert.Equal(details, decision.Payload.AdvisoryBoardDecisionDetails);
	}
}
