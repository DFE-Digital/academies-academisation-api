using System;
using System.Linq;
using Bogus;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
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
			default,
			_faker.Random.Int(1, 1000),
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new(),
			null,
			new() {AdvisoryBoardDeferredReason.PerformanceConcerns},
			null,
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
			default,
			_faker.Random.Int(1, 1000),
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new(),
			null,
			new() {AdvisoryBoardDeferredReason.PerformanceConcerns},
			null,
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
			DeclinedReasons = new(
				expectedDetails.DeclinedReasons!
					.Select(reason => new ConversionAdvisoryBoardDecisionDeclinedReasonState
					{
						Reason = reason
					})),
			DeclinedOtherReason = expectedDetails.DeclinedOtherReason,
			DeferredReasons = new(
				expectedDetails.DeferredReasons!
					.Select(reason => new ConversionAdvisoryBoardDecisionDeferredReasonState
					{
						Reason = reason
					})),
			DeferredOtherReason = expectedDetails.DeferredOtherReason,
			AdvisoryBoardDecisionDate = expectedDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = expectedDetails.DecisionMadeBy
		};

		//Act
		var result = ConversionAdvisoryBoardDecisionState.MapFromDomain(mockDecision.Object);
		
		//assert
		Assert.Equivalent(expected, result);
	}
}
