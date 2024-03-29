﻿using System;
using System.Collections.Generic;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionValidatorTests
{
	private readonly Faker _faker = new();

	private const int ConversionProjectId = 1;

	private readonly ConversionAdvisoryBoardDecisionValidator _validator = new();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsApproved_AndDetailsAreValid___ReturnsValidResult(bool approvedConditionsSet)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			approvedConditionsSet,
			approvedConditionsSet ? _faker.Lorem.Sentence() : null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act
		var result = _validator.Validate(details);

		//Assert
		Assert.True(result.IsValid);

	}


	[Fact]
	private void DecisionIsDeclined_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardDeclinedReasonDetails> declinedReasons = new()
		{
			new(AdvisoryBoardDeclinedReason.Other, _faker.Lorem.Sentence()),
			new(AdvisoryBoardDeclinedReason.Finance, _faker.Lorem.Sentence())
		};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			declinedReasons,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act
		var result = _validator.Validate(details);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void DecisionIsDeferred_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardDeferredReasonDetails> deferredReasons = new()
		{
			new(AdvisoryBoardDeferredReason.Other, _faker.Lorem.Sentence()),
			new(AdvisoryBoardDeferredReason.PerformanceConcerns, _faker.Lorem.Sentence())
		};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			deferredReasons,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>());

		// Act
		var result = _validator.Validate(details);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void DecisionIsWithdrawn_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons = new()
		{
			new(AdvisoryBoardWithdrawnReason.Other, _faker.Lorem.Sentence()),
			new(AdvisoryBoardWithdrawnReason.PerformanceConcerns, _faker.Lorem.Sentence())
		};

		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Withdrawn,
			null,
			null,
			null,
			null,
			withdrawnReasons,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			null,
			default,
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			true,
			value,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			_faker.Lorem.Sentence(),
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsApproved_DeclinedReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) },
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsApproved_AndDeferredReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange	
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) },
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsApproved_AndWithdrawnReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange	
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardWithdrawnReason>(), _faker.Lorem.Sentence()) },
			DateTime.UtcNow.AddDays(-1),
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
					 == nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.WithdrawnReasons))
		);
	}

	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsIsNull___ReturnsInvalidResult()
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new(),
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsDeclined_DeclinedReasonsContainsDetails_WithEmptyDetailsString___ReturnsInvalidResult(
		string declinedReasonDetails)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), declinedReasonDetails) },
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
					 == $"{nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeclinedReasons)}[0]."
					 + $"{nameof(AdvisoryBoardDeclinedReasonDetails.Details)}")
		);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	private void DecisionIsWithdrawn_WithdrawnReasonsContainsDetails_WithEmptyDetailsString___ReturnsInvalidResult(
		string withdrawnReasonDetails)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Withdrawn,
			null,
			null,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardWithdrawnReason>(), withdrawnReasonDetails) },
			DateTime.UtcNow.AddDays(-1),
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
					 == $"{nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.WithdrawnReasons)}[0]."
					 + $"{nameof(AdvisoryBoardWithdrawnReasonDetails.Details)}")
		);
	}


	[Fact]
	private void DecisionIsDeclined_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) },
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsDeclined_DeferredReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) },
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			null,
			new(),
			null,
			DateTime.UtcNow.AddDays(-1),
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
	private void DecisionIsDeferred_DeferredReasonsContainsDetails_WithEmptyDetailsString___ReturnsInvalidResult(
		string deferredReasonDetails)
	{
		// Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), deferredReasonDetails) },
			null,
			DateTime.UtcNow.AddDays(-1),
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
					 == $"{nameof(IConversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails.DeferredReasons)}[0]."
					 + $"{nameof(AdvisoryBoardDeferredReasonDetails.Details)}")
		);
	}

	[Fact]
	private void DecisionIsDeferred_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		AdvisoryBoardDecisionDetails details = new(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) },
			null,
			DateTime.UtcNow.AddDays(-1),
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
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			new() { new(_faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) },
			new() { new(_faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) },
			null,
			DateTime.UtcNow.AddDays(-1),
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
