using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionCreateTests
{
	private readonly Faker _faker = new();
	private const int ConversionProjectId = 1;
	
	private readonly ConversionAdvisoryBoardDecisionFactory _target = new();

	private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsApproved_AndDetailsAreValid___ReturnCreateSuccessResult(bool approvedConditionsSet)
	{
		// Arrange
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
		var result = (CreateSuccessResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(result.Payload);
		Assert.Equal(details, result.Payload.AdvisoryBoardDecisionDetails);
	}
	
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsDeclined_AndDetailsAreValid___ReturnCreateSuccessResult(bool declinedOtherSet)
	{
		// Arrange
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
		var result = _target.Create(details);

		var test = (CreateSuccessResult<IConversionAdvisoryBoardDecision>) result;

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(test.Payload);
		Assert.Equal(details, test.Payload.AdvisoryBoardDecisionDetails);
	}
	
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsDeferred_AndDetailsAreValid___ReturnCreateSuccessResult(
		bool deferredOtherSet)
	{
		// Arrange
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
		var result = (CreateSuccessResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.IsType<ConversionAdvisoryBoardDecision>(result.Payload);
		Assert.Equal(details, result.Payload.AdvisoryBoardDecisionDetails);
	}
	
	[Fact]
	private void AdvisoryBoardDecisionDateIsDefault___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
		
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate));
	}
	
	[Fact]
	private void AdvisoryBoardDecisionDateIsFutureDate___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
	
		
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate));
	}
	
	[Fact]
	private void DecisionIsApproved_WhenApprovedConditionsSetIsNull___ReturnsCreateValidationErrorResult()
	{
		// Arrange
		
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet));
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsApproved__WhenApprovedConditionsSetIsTrue_AndApprovedConditionsDetailsIsEmpty___ReturnsCreateValidationErrorResult(
			string? value)
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails));
	}
	
	[Fact]
	private void DecisionIsApproved__WhenApprovedConditionsSetIsFalse_AndApprovedConditionsDetailsIsNotEmpty___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails));
	}
	
	[Fact]
	private void DecisionIsApproved_AndDeclinedReasonsIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons));
	}
	
	[Fact]
	private void DecisionIsApproved_AndDeferredReasonsIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange	
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons));
	}
	
	[Fact]
	private void DecisionIsDeclined_WhenDeclinedReasonsIsNull___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons));
	}
	
	[Fact]
	private void DecisionIsDeclined_WhenDeclinedReasonsIsEmpty___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons));
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsDeclined__WhenDeclinedReasonsContainsOther_AndDeclinedOtherReasonIsEmpty___ReturnsCreateValidationErrorResult(string declinedOtherReason)
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedOtherReason));
	}
	
	[Fact]
	private void DecisionIsDeclined__WhenDeclinedReasonsDoesNotContainOther_AndDeclinedOtherReasonIsNotEmpty___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedOtherReason));
	}
	
	[Fact]
	private void DecisionIsDeclined_AndApprovedConditionsSetIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet));
	}
	
	[Fact]
	private void DecisionIsDeclined_AndDeferredReasonsIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons));
	}
	
	
	[Fact]
	private void DecisionIsDeferred_WhenDeferredReasonsIsNull___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons));
	}
	
	[Fact]
	private void DecisionIsDeferred_WhenDeferredReasonsIsEmpty___ReturnsCreateValidationErrorResult()
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons));
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsDeferred__WhenDeferredReasonsContainsOther_AndDeferredOtherReasonIsEmpty___ReturnsCreateValidationErrorResult(string declinedOtherReason)
	{
		// Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredOtherReason));
	}
	
	[Fact]
	private void DecisionIsDeferred__WhenDeferredReasonsDoesNotContainOther_AndDeferredOtherReasonIsNotEmpty___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredOtherReason));
	}
	
	[Fact]
	private void DecisionIsDeferred_AndApprovedConditionsSetIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet));
	}
	
	[Fact]
	private void DecisionIsDeferred_AndDeclinedReasonsIsNotNull___ReturnsCreateValidationErrorResult()
	{
		//Arrange
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
	
		// Act
		var result = (CreateValidationErrorResult<IConversionAdvisoryBoardDecision>) _target.Create(details);

		//Assert
		Assert.NotEmpty(result.ValidationErrors);
		Assert.Contains(result.ValidationErrors, e =>
			e.PropertyName == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons));
	}
}
