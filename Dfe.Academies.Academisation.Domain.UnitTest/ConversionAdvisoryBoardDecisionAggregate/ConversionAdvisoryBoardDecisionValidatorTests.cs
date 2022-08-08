using System;
using System.Collections.Generic;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionValidatorTests
{
	private readonly Faker _faker = new();
	private const int ConversionProjectId = 1;
	
	private readonly ConversionAdvisoryBoardDecisionValidator _validator = new();
	private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsApproved_AndDetailsAreValid___ReturnsValidResult(bool approvedConditionsSet)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);
		
		//Assert
		Assert.True(result.IsValid);

	}
	
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsDeclined_AndDetailsAreValid___ReturnsValidResult(bool declinedOtherSet)
	{
		// Arrange
		List<AdvisoryBoardDeclinedReason> declinedReasons = declinedOtherSet
			? new() {AdvisoryBoardDeclinedReason.Other}
			: new() {AdvisoryBoardDeclinedReason.Performance};
	
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);
		
		//Assert
		Assert.True(result.IsValid);
	}
	
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsDeferred_AndDetailsAreValid___ReturnsValidResult(bool deferredOtherSet)
	{
		// Arrange
		List<AdvisoryBoardDeferredReason> deferredReasons = deferredOtherSet
			? new() {AdvisoryBoardDeferredReason.Other}
			: new() {AdvisoryBoardDeferredReason.PerformanceConcerns};
	
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);
		
		//Assert
		Assert.True(result.IsValid);
	}
	
	[Fact]
	private void AdvisoryBoardDecisionDateIsDefault___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				   == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate))
			);
	}
	
	[Fact]
	private void AdvisoryBoardDecisionDateIsFutureDate___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate))
		);
	}
	
	[Fact]
	private void DecisionIsApproved_WhenApprovedConditionsSetIsNull___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet))
		);
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsApproved_ApprovedConditionsSetIsTrue_ApprovedConditionsDetailsIsEmpty___ReturnsInvalidResult(
		string? value)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails))
		);
	}
	
	[Fact]
	private void DecisionIsApproved_ApprovedConditionsSetIsFalse_ApprovedConditionsDetailsIsNotEmpty___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails))
		);
	}
	
	[Fact]
	private void DecisionIsApproved_DeclinedReasonsIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName 
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons))
		);
	}
	
	[Fact]
	private void DecisionIsApproved_AndDeferredReasonsIsNotNull___ReturnsInvalidResult()
	{
		//Arrange	
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons))
		);
	}
	
	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsIsNull___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons))
		);
	}
	
	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsIsEmpty___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons))
		);
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsDeclined_DeclinedReasonsContainsOther_DeclinedOtherReasonIsEmpty___ReturnsInvalidResult(
		string declinedOtherReason)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedOtherReason))
		);
	}
	
	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsDoesNotContainOther_DeclinedOtherReasonIsNotEmpty___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedOtherReason))
		);
	}
	
	[Fact]
	private void DecisionIsDeclined_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet))
		);
	}
	
	[Fact]
	private void DecisionIsDeclined_DeferredReasonsIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons))
		);
	}
	
	
	[Fact]
	private void DecisionIsDeferred_DeferredReasonsIsNull___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons))
		);
	}
	
	[Fact]
	private void DecisionIsDeferred_DeferredReasonsIsEmpty___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons))
		);
	}
	
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsDeferred_DeferredReasonsContainsOther_DeferredOtherReasonIsEmpty___ReturnsInvalidResult(
		string declinedOtherReason)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredOtherReason))
		);
	}
	
	[Fact]
	private void DecisionIsDeferred_DeferredReasonsDoesNotContainOther_DeferredOtherReasonIsNotEmpty___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredOtherReason))
		);
	}
	
	[Fact]
	private void DecisionIsDeferred_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet))
		);
	}
	
	[Fact]
	private void DecisionIsDeferred_DeclinedReasonsIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			default,
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
		var result = _validator.Validate(details);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				     == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons))
		);
	}
}
