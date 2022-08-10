using Dfe.Academies.Academisation.Domain.Core;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionValidator : AbstractValidator<AdvisoryBoardDecisionDetails>
{
	public ConversionAdvisoryBoardDecisionValidator()
	{
		ValidateApprovedDecision();
		ValidateDeclinedDecision();
		ValidateDeferredDecision();
		ValidateDecisionDate();
	}

	private static string NotNullMessage(string target)
	{
		return $"{target} should not be null";
	}

	private static string NullMessage(string target)
	{
		return $"{target} should be null";
	}

	private static string WhenIsNotSuffix(string target, string value)
	{
		return $" when {target} is not {value}";
	}

	private static string WhenIsSuffix(string target, string value)
	{
		return $" when {target} is {value}";
	}

	private static string NotEmptyMessage(string target)
	{
		return $"{target} should not be null or empty";
	}

	private static string EmptyMessage(string target)
	{
		return $"{target} should be null or empty";
	}

	private static string AndSuffix(string conditional, string value)
	{
		return $" and {conditional} is {value}";
	}

	private static string OrSuffix(string conditional, string value)
	{
		return $" or {conditional} is {value}";
	}

	private static string AndContainsSuffix(string target, string value)
	{
		return $" and {target} contains {value}";
	}

	private static string AndNotContainsSuffix(string target, string value)
	{
		return $" and {target} does not contain {value}";
	}

	private static string OrNotContainsSuffix(string target, string value)
	{
		return $" or {target} does not contain {value}";
	}

	private static string NotSetDateMessage(string target)
	{
		return $"{target} must be set to a valid date";
	}

	private static string FutureDateMessage(string target)
	{
		return $"{target} must not be in the future";
	}

	private void ValidateApprovedDecision()
	{
		RuleFor(details => details.ApprovedConditionsSet)
			.NotNull()
			.When(details => details.Decision is AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NotNullMessage(
					nameof(details.ApprovedConditionsSet)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.ApprovedConditionsSet)
			.Null()
			.When(details => details.Decision is not AdvisoryBoardDecision.Approved)
			.WithMessage(details =>
				NullMessage(
					nameof(details.ApprovedConditionsSet)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)));

		RuleFor(details => details.ApprovedConditionsDetails)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecision.Approved &&
				details.ApprovedConditionsSet is true)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				AndSuffix(
					nameof(details.ApprovedConditionsSet),
					bool.TrueString));

		RuleFor(details => details.ApprovedConditionsDetails)
			.Empty()
			.When(details =>
				details.Decision is not AdvisoryBoardDecision.Approved ||
				details.ApprovedConditionsSet is false)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.ApprovedConditionsDetails)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Approved)) +
				OrSuffix(
					nameof(details.ApprovedConditionsSet),
					bool.FalseString));
	}

	private void ValidateDeclinedDecision()
	{
		RuleFor(details => details.DeclinedReasons)
			.NotNull()
			.NotEmpty()			
			.When(details => details.Decision is AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleFor(details => details.DeclinedReasons)
			.Empty()
			.When(details => details.Decision is not AdvisoryBoardDecision.Declined)
			.WithMessage(details =>
				EmptyMessage(
					nameof(details.DeclinedReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)));

		RuleFor(details => details.DeclinedOtherReason)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecision.Declined &&
				details.DeclinedReasons is not null &&
				details.DeclinedReasons.Contains(AdvisoryBoardDeclinedReason.Other))
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeclinedOtherReason)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)) +
				AndContainsSuffix(
					nameof(details.DeclinedReasons),
					nameof(AdvisoryBoardDeclinedReason.Other)));

		RuleFor(details => details.DeclinedOtherReason)
			.Null()
			.When(details =>
				details.Decision is not AdvisoryBoardDecision.Declined ||
				(details.DeclinedReasons is not null &&
				 !details.DeclinedReasons.Contains(AdvisoryBoardDeclinedReason.Other)))
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeclinedOtherReason)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Declined)) +
				OrNotContainsSuffix(
					nameof(details.DeclinedReasons),
					nameof(AdvisoryBoardDeclinedReason.Other)));
	}

	private void ValidateDeferredDecision()
	{
		RuleFor(details => details.DeferredReasons)
			.NotNull()
			.NotEmpty()
			.When(details => details.Decision is AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredReasons)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Deferred)));

		RuleFor(details => details.DeferredReasons)
			.Empty()
			.When(details => details.Decision is not AdvisoryBoardDecision.Deferred)
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeferredReasons)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Deferred)));

		RuleFor(details => details.DeferredOtherReason)
			.NotEmpty()
			.When(details =>
				details.Decision is AdvisoryBoardDecision.Deferred &&
				details.DeferredReasons is not null &&
				details.DeferredReasons!.Contains(AdvisoryBoardDeferredReason.Other))
			.WithMessage(details =>
				NotEmptyMessage(
					nameof(details.DeferredOtherReason)) +
				WhenIsSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Deferred)) +
				AndContainsSuffix(
					nameof(details.DeferredReasons),
					nameof(AdvisoryBoardDeferredReason.Other)));

		RuleFor(details => details.DeferredOtherReason)
			.Null()
			.When(details =>
				details.Decision is not AdvisoryBoardDecision.Deferred ||
				(details.DeferredReasons is not null &&
				 !details.DeferredReasons.Contains(AdvisoryBoardDeferredReason.Other)))
			.WithMessage(details =>
				NullMessage(
					nameof(details.DeferredOtherReason)) +
				WhenIsNotSuffix(
					nameof(details.Decision),
					nameof(AdvisoryBoardDecision.Deferred)) +
				AndNotContainsSuffix(
					nameof(details.DeferredReasons),
					nameof(AdvisoryBoardDeferredReason.Other)));
	}

	private void ValidateDecisionDate()
	{
		RuleFor(details => details.AdvisoryBoardDecisionDate)
			.Must(date => date != default)
			.WithMessage(details =>
				NotSetDateMessage(
					nameof(details.AdvisoryBoardDecisionDate)));

		RuleFor(details => details.AdvisoryBoardDecisionDate)
			.Must(date => date < DateTime.UtcNow)
			.WithMessage(details =>
				FutureDateMessage(
					nameof(details.AdvisoryBoardDecisionDate)));
	}
}
