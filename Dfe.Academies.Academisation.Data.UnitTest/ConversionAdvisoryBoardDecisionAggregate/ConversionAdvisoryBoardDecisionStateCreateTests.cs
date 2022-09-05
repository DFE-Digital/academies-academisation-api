using System;
using System.Linq;
using Bogus;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateCreateTests
{
	private readonly Faker _faker;

	public ConversionAdvisoryBoardDecisionStateCreateTests()
	{
		_faker = new();
	}

	[Fact]
	public void ShouldReturnConversionAdvisoryBoardDecisionState()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			_faker.Random.Int(1, 1000),
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence())
			},
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
		mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(details);

		//Act
		var result = ConversionAdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecisionState>(result);
	}

	[Fact]
	public void ShouldReturnExpectedConversionAdvisoryBoardDecisionState()
	{
		//Arrange
		AdvisoryBoardDecisionDetails expectedDetails = new(
			_faker.Random.Int(1, 1000),
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence())
			},
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		var mockDecision = new Mock<IConversionAdvisoryBoardDecision>();
		mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(expectedDetails);

		var expected = new ConversionAdvisoryBoardDecisionState
		{
			ConversionProjectId = expectedDetails.ConversionProjectId,
			Decision = expectedDetails.Decision,
			ApprovedConditionsSet = expectedDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = expectedDetails.ApprovedConditionsDetails,
			DeclinedReasons = null,
			DeferredReasons = new(
				expectedDetails.DeferredReasons!
					.Select(reason => new ConversionAdvisoryBoardDecisionDeferredReasonState
					{
						Reason = reason.Reason,
						Details = reason.Details,
					})),
			AdvisoryBoardDecisionDate = expectedDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = expectedDetails.DecisionMadeBy
		};

		//Act
		var result = ConversionAdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);

		//assert
		Assert.Equivalent(expected, result);
	}
}
