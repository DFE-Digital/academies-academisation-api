using System;
using System.Linq;
using Bogus;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateMapFromDomainTests
{
	private readonly Faker _faker = new();

	[Fact]
	public void ShouldReturnConversionAdvisoryBoardDecisionState()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			_faker.Random.Int(1, 1000),
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence())
			},
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		Mock<IConversionAdvisoryBoardDecision> mockDecision = new();
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
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence())
			},
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		Mock<IConversionAdvisoryBoardDecision> mockDecision = new();
		mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(expectedDetails);

		ConversionAdvisoryBoardDecisionState expected = new()
		{
			ConversionProjectId = expectedDetails.ConversionProjectId,
			Decision = expectedDetails.Decision,
			ApprovedConditionsSet = expectedDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = expectedDetails.ApprovedConditionsDetails,
			DeclinedReasons = new(
				expectedDetails.DeclinedReasons!
					.Select(reason => new ConversionAdvisoryBoardDecisionDeclinedReasonState
					{
						Reason = reason.Reason,
						Details = reason.Details
					})),
			DeferredReasons = null,
			AdvisoryBoardDecisionDate = expectedDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = expectedDetails.DecisionMadeBy
		};

		//Act
		var result = ConversionAdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);

		//assert
		Assert.Equivalent(expected, result);
	}
}
