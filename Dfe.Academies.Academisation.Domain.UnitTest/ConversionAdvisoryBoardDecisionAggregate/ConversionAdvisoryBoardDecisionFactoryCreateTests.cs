using System;
using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionFactoryTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();
	
	private const int ConversionProjectId = 1;
	
	private readonly ConversionAdvisoryBoardDecisionFactory _target = new();

	private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');

	[Fact]
	public void DecisionIsInvalid___ReturnsValidationErrorResult()
	{
		//Arrange
		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();

		//Act
		var result = _target.Create(details);

		//Assert
		Assert.IsType<CreateValidationErrorResult<IConversionAdvisoryBoardDecision>>(result);
	}
	
	[Fact]
	public void DecisionIsValid___ReturnsCreateSuccessResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			true,
			GetRandomString,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		//Act
		var result = _target.Create(details);

		//Assert
		var decision = Assert.IsType<CreateSuccessResult<IConversionAdvisoryBoardDecision>>(result);
		Assert.Equal(details, decision.Payload.AdvisoryBoardDecisionDetails);
	}
}
