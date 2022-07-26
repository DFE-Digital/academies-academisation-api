using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionCreateTests
{
	private readonly Faker _faker = new();
	private const int ConversionProjectId = 1;

	private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsApproved_AndDetailsAreValid___ReturnConversionAdvisoryBoardDecision(
		bool approvedConditionsSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			approvedConditionsSet,
			approvedConditionsSet ? GetRandomString : null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act
		var result = await target.Create(details);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(result);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsApproved_AndDetailsAreValid___SetsExpectedAdvisoryBoardDecisionDetails(
		bool approvedConditionsSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			approvedConditionsSet,
			approvedConditionsSet ? GetRandomString : null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act 
		var result = await target.Create(details);

		//Assert
		Assert.Equal(details, result.AdvisoryBoardDecisionDetails);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsDeclined_AndDetailsAreValid___ReturnsConversionAdvisoryBoardDecision(
		bool declinedOtherSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		List<AdvisoryBoardDeclinedReason> declinedReasons = declinedOtherSet
			? new() {AdvisoryBoardDeclinedReason.Other}
			: new() {AdvisoryBoardDeclinedReason.Performance};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			declinedReasons,
			declinedOtherSet ? GetRandomString : null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act 
		var result = await target.Create(details);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(result);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsDeclined_AndDetailsAreValid___SetsExpectedAdvisoryBoardDecisionDetails(
		bool declinedOtherSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		List<AdvisoryBoardDeclinedReason> declinedReasons = declinedOtherSet
			? new() {AdvisoryBoardDeclinedReason.Other}
			: new() {AdvisoryBoardDeclinedReason.Performance};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			declinedReasons,
			declinedOtherSet ? GetRandomString : null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act 
		var result = await target.Create(details);

		//Assert
		Assert.Equal(details, result.AdvisoryBoardDecisionDetails);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsDeferred_AndDetailsAreValid___ReturnsConversionAdvisoryBoardDecision(
		bool deferredOtherSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		List<AdvisoryBoardDeferredReason> deferredReasons = deferredOtherSet
			? new() {AdvisoryBoardDeferredReason.Other}
			: new() {AdvisoryBoardDeferredReason.PerformanceConcerns};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			deferredReasons,
			deferredOtherSet ? GetRandomString : null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act 
		var result = await target.Create(details);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(result);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private async Task DecisionIsDeferred_AndDetailsAreValid__SetsExpectedAdvisoryBoardDecision(
		bool deferredOtherSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		List<AdvisoryBoardDeferredReason> deferredReasons = deferredOtherSet
			? new() {AdvisoryBoardDeferredReason.Other}
			: new() {AdvisoryBoardDeferredReason.PerformanceConcerns};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			deferredReasons,
			deferredOtherSet ? GetRandomString : null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act 
		var result = await target.Create(details);

		//Assert
		Assert.Equal(details, result.AdvisoryBoardDecisionDetails);
	}

	[Fact]
	private async Task AdvisoryBoardDecisionDateIsDefault___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			null,
			null,
			default,
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task AdvisoryBoardDecisionDateIsFutureDate___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsApproved_WhenApprovedConditionsSetIsNull___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			null,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private async Task
		DecisionIsApproved__WhenApprovedConditionsSetIsTrue_AndApprovedConditionsDetailsIsEmpty___ThrowsException(
			string? value)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			true,
			value,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task
		DecisionIsApproved__WhenApprovedConditionsSetIsFalse_AndApprovedConditionsDetailsIsNotEmpty___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			false,
			GetRandomString,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsApproved_AndDeclinedReasonsIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			new(),
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsApproved_AndDeferredReasonsIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			new(),
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeclined_WhenDeclinedReasonsIsNull___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeclined_WhenDeclinedReasonsIsEmpty___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new(),
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private async Task
		DecisionIsDeclined__WhenDeclinedReasonsContainsOther_AndDeclinedOtherReasonIsEmpty___ThrowsException(
			string declinedOtherReason)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new() {AdvisoryBoardDeclinedReason.Other},
			declinedOtherReason,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task
		DecisionIsDeclined__WhenDeclinedReasonsDoesNotContainOther_AndDeclinedOtherReasonIsNotEmpty___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new() {AdvisoryBoardDeclinedReason.Performance},
			GetRandomString,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeclined_AndApprovedConditionsSetIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			new() {AdvisoryBoardDeclinedReason.Performance},
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeclined_AndDeferredReasonsIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			null,
			new(),
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}


	[Fact]
	private async Task DecisionIsDeferred_WhenDeferredReasonsIsNull___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeferred_WhenDeferredReasonsIsEmpty___ThrowsException()
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			new(),
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private async Task
		DecisionIsDeferred__WhenDeferredReasonsContainsOther_AndDeferredOtherReasonIsEmpty___ThrowsException(
			string declinedOtherReason)
	{
		// Arrange
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			new() {AdvisoryBoardDeferredReason.Other},
			declinedOtherReason,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task
		DecisionIsDeferred__WhenDeferredReasonsDoesNotContainOther_AndDeferredOtherReasonIsNotEmpty___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			new() {AdvisoryBoardDeferredReason.PerformanceConcerns},
			GetRandomString,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeferred_AndApprovedConditionsSetIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			null,
			null,
			new() {AdvisoryBoardDeferredReason.PerformanceConcerns},
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}

	[Fact]
	private async Task DecisionIsDeferred_AndDeclinedReasonsIsNotNull___ThrowsException()
	{
		ConversionAdvisoryBoardDecisionFactory target = new();


		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new(),
			null,
			new() {AdvisoryBoardDeferredReason.PerformanceConcerns},
			null,
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act & Assert
		await Assert.ThrowsAsync<ValidationException>(() => target.Create(details));
	}
}
