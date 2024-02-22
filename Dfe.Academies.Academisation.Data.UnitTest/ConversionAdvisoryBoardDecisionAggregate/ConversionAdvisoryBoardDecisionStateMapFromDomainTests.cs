using System;
using System.Linq;
using Bogus;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
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
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence())
			},
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		Mock<IConversionAdvisoryBoardDecision> mockDecision = new();
		mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(details);

		//Act
		var result = AdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);

		//Assert
		Assert.IsType<AdvisoryBoardDecisionState>(result);
	}

	[Fact]
	public void ShouldReturnExpectedConversionAdvisoryBoardDecisionState()
	{
		//Arrange
		AdvisoryBoardDecisionDetails expectedDetails = new(
			_faker.Random.Int(1, 1000),
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new()
			{
				new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence())
			},
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>()
		);

		Mock<IConversionAdvisoryBoardDecision> mockDecision = new();
		mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(expectedDetails);

		AdvisoryBoardDecisionState expected = new()
		{
			ConversionProjectId = expectedDetails.ConversionProjectId,
			Decision = expectedDetails.Decision,
			ApprovedConditionsSet = expectedDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = expectedDetails.ApprovedConditionsDetails,
			DeclinedReasons = new(
				expectedDetails.DeclinedReasons!
					.Select(reason => new AdvisoryBoardDecisionDeclinedReasonState
					{
						Reason = reason.Reason,
						Details = reason.Details
					})),
			DeferredReasons = null,
			AdvisoryBoardDecisionDate = expectedDetails.AdvisoryBoardDecisionDate,
			AcademyOrderDate = expectedDetails.AcademyOrderDate,
			DecisionMadeBy = expectedDetails.DecisionMadeBy
		};

		//Act
		var result = AdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);

		//assert
		Assert.Equivalent(expected, result);
	}
}
