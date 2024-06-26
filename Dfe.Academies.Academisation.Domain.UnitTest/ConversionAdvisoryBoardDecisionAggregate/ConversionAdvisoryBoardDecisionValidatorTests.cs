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
	private const int TransferProjectId = 2;

	private readonly ConversionAdvisoryBoardDecisionValidator _validator = new();

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	private void DecisionIsApproved_AndDetailsAreValid___ReturnsValidResult(bool approvedConditionsSet)
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			approvedConditionsSet,
			approvedConditionsSet ? _faker.Lorem.Sentence() : null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);

	}


	[Fact]
	private void DecisionIsDeclined_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardDeclinedReasonDetails> declinedReasons = new()
		{
			new(1, AdvisoryBoardDeclinedReason.Other, _faker.Lorem.Sentence()),
			new(1, AdvisoryBoardDeclinedReason.Finance, _faker.Lorem.Sentence())
		};

		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, declinedReasons, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void DecisionIsDeferred_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardDeferredReasonDetails> deferredReasons = new()
		{
			new(1, AdvisoryBoardDeferredReason.Other, _faker.Lorem.Sentence()),
			new(1, AdvisoryBoardDeferredReason.PerformanceConcerns, _faker.Lorem.Sentence())
		};

		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), deferredReasons, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void DecisionIsWithdrawn_AndDetailsAreValid___ReturnsValidResult()
	{
		// Arrange
		List<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons = new()
		{
			new(1, AdvisoryBoardWithdrawnReason.Other, _faker.Lorem.Sentence()),
			new(1, AdvisoryBoardWithdrawnReason.PerformanceConcerns, _faker.Lorem.Sentence())
		};

		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Withdrawn,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, withdrawnReasons, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void AdvisoryBoardDecisionDateIsDefault___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			default,
			default,
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
				   == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)}")
			);
	}

	[Fact]
	private void AdvisoryBoardDecisionDateIsFutureDate___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(1),
			DateTime.UtcNow.AddDays(1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate)}")
		);
	}

	[Fact]
	private void DecisionIsApproved_WhenApprovedConditionsSetIsNull___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.ApprovedConditionsSet)}")
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
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			true,
			value,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName ==
					 $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)}")
		);
	}

	[Fact]
	private void DecisionIsApproved_ApprovedConditionsSetIsFalse_ApprovedConditionsDetailsIsNotEmpty___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			_faker.Lorem.Sentence(),
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.ApprovedConditionsDetails)}")
		);
	}

	[Fact]
	private void DecisionIsApproved_DeclinedReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, new List<AdvisoryBoardDeclinedReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) }, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeclinedReasons))
		);
	}

	[Fact]
	private void DecisionIsApproved_AndDeferredReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange	
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), new List<AdvisoryBoardDeferredReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) }, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeferredReasons))
		);
	}

	[Fact]
	private void DecisionIsApproved_AndWithdrawnReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange	
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, new List<AdvisoryBoardWithdrawnReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardWithdrawnReason>(), _faker.Lorem.Sentence()) }, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.WithdrawnReasons))
		);
	}

	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsIsNull___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeclinedReasons))
		);
	}

	[Fact]
	private void DecisionIsDeclined_DeclinedReasonsIsEmpty___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, new List<AdvisoryBoardDeclinedReasonDetails>(), null, null, DateTime.UtcNow, DateTime.UtcNow); ;

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeclinedReasons))
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
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, new List<AdvisoryBoardDeclinedReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeclinedReason>(), declinedReasonDetails) }, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(IConversionAdvisoryBoardDecision.DeclinedReasons)}[0]."
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
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Withdrawn,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, new List<AdvisoryBoardWithdrawnReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardWithdrawnReason>(), withdrawnReasonDetails) }, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(IConversionAdvisoryBoardDecision.WithdrawnReasons)}[0]."
					 + $"{nameof(AdvisoryBoardWithdrawnReasonDetails.Details)}")
		);
	}


	[Fact]
	private void DecisionIsDeclined_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, new List<AdvisoryBoardDeclinedReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) }, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.ApprovedConditionsSet)}")
		);
	}

	[Fact]
	private void DecisionIsDeclined_DeferredReasonsIsNotNullOrEmpty___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), new List<AdvisoryBoardDeferredReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) }, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(ConversionAdvisoryBoardDecision.DeferredReasons)}")
		);
	}

	[Fact]
	private void DecisionIsDeferred_DeferredReasonsIsNull___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeferredReasons))
		);
	}

	[Fact]
	private void DecisionIsDeferred_DeferredReasonsIsEmpty___ReturnsInvalidResult()
	{
		// Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), new List<AdvisoryBoardDeferredReasonDetails>(), null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeferredReasons))
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
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), new List<AdvisoryBoardDeferredReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeferredReason>(), deferredReasonDetails) }, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(IConversionAdvisoryBoardDecision.DeferredReasons)}[0]."
					 + $"{nameof(AdvisoryBoardDeferredReasonDetails.Details)}")
		);
	}

	[Fact]
	private void DecisionIsDeferred_ApprovedConditionsSetIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Declined,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()), new List<AdvisoryBoardDeferredReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) }, null, null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.ApprovedConditionsSet)}")
		);
	}

	[Fact]
	private void DecisionIsDeferred_DeclinedReasonsIsNotNull___ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Deferred,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			_faker.PickRandom<DecisionMadeBy>(),
			_faker.Name.ToString()),
			new List<AdvisoryBoardDeferredReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeferredReason>(), _faker.Lorem.Sentence()) },
			new List<AdvisoryBoardDeclinedReasonDetails>() { new(1, _faker.PickRandom<AdvisoryBoardDeclinedReason>(), _faker.Lorem.Sentence()) },
			null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == nameof(IConversionAdvisoryBoardDecision.DeclinedReasons))
		);
	}

	[Fact]
	private void DecisionMadeByNotNone_WithEmptyDecisionMakerName_ReturnsInvalidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			DecisionMadeBy.DeputyDirector,
			string.Empty),
			null,
			null,
			null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.Multiple(
			() => Assert.False(result.IsValid),
			() => Assert.NotEmpty(result.Errors),
			() => Assert.Contains(result.Errors,
				e => e.PropertyName
					 == $"{nameof(AdvisoryBoardDecisionDetails)}.{nameof(AdvisoryBoardDecisionDetails.DecisionMakerName)}")
		);
	}

	[Fact]
	private void DecisionMadeByNotNone_WithyDecisionMakerName_ReturnsValidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			DecisionMadeBy.DeputyDirector,
			"John Smith"),
			null,
			null,
			null,
			null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	private void DecisionMadeByIsNone_WithEmptyDecisionMakerName_ReturnsValidResult()
	{
		//Arrange
		ConversionAdvisoryBoardDecision decision = new(1, new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			null,
			AdvisoryBoardDecision.Approved,
			false,
			null,
			DateTime.UtcNow.AddDays(-1),
			DateTime.UtcNow.AddDays(-1),
			DecisionMadeBy.None,
			string.Empty),
			null,
			null,
			null, null, DateTime.UtcNow, DateTime.UtcNow);

		// Act
		var result = _validator.Validate(decision);

		//Assert
		Assert.True(result.IsValid);
	}
}
